// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Arduino.OpenWeather.Threading.Core
{
    public sealed class BackgroundJobManager : IBackgroundJobManager
    {
        private const int SLEEP_INTERVAL = 100;

        private static readonly object _lockObject = new object();

        private readonly IBackgroundJobFactory _jobFactory;

        private readonly ConcurrentDictionary<string, JobInfo> _jobStore;

        private readonly CancellationToken _token;
        private readonly CancellationTokenSource _tokenSource;

        public BackgroundJobManager(IBackgroundJobFactory jobFactory)
        {
            this._jobFactory = jobFactory;

            this._jobStore = new ConcurrentDictionary<string, JobInfo>();

            this._tokenSource = new CancellationTokenSource();
            this._token = this._tokenSource.Token;

            ThreadPool.QueueUserWorkItem(JobStoreCleanup, this, false);
        }

        public IEnumerable<JobInfo> GetActiveJobs()
        {
            return this._jobStore.Values.ToList();
        }

        public void Start<T>(string id, string options)
            where T : IBackgroundJob
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (this._jobStore.ContainsKey(id))
            {
                throw new InvalidOperationException($@"Job with id {id} is already registered.");
            }

            var jobInfo = new JobInfo
            {
                Id = id,
                Job = this._jobFactory.Create<T>(options),
                Options = options,
                CancellationTokenSource = new CancellationTokenSource(),
                State = JobState.Idle
            };

            this._token.Register(() => jobInfo.CancellationTokenSource.Cancel());

            lock (_lockObject)
            {
                this._jobStore.TryAdd(id, jobInfo);
            }

            ThreadPool.QueueUserWorkItem(SpawnThread, jobInfo, false);
        }

        public void Stop(string id)
        {
            if (!this._jobStore.ContainsKey(id))
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            var jobInfo = this._jobStore[id];

            jobInfo.CancellationTokenSource.Cancel();
        }

        private static void JobStoreCleanup(BackgroundJobManager jobManager)
        {
            var endedStatuses = new[] { JobState.Ended, JobState.Failed, JobState.Stopped };

            while (!jobManager._tokenSource.IsCancellationRequested)
            {
                var endedJobs = jobManager._jobStore.Values
                    .Where(x => endedStatuses.Contains(x.State))
                    .Select(x => x.Id)
                    .ToList();

                foreach (var job in endedJobs)
                {
                    lock (_lockObject)
                    {
                        jobManager._jobStore.TryRemove(job, out var jobInfo);
                    }
                }

                try
                {
                    jobManager._token.WaitHandle.WaitOne(SLEEP_INTERVAL);
                }
                catch(Exception)
                {

                }
            }
        }

        private static void SpawnThread(JobInfo jobInfo)
        {
            var endedTaskStatuses = new[] {TaskStatus.Faulted, TaskStatus.RanToCompletion, TaskStatus.Canceled};

            Task task;

            lock (_lockObject)
            {
                jobInfo.State = JobState.Running;

                task = jobInfo.Job.ExecuteAsync(jobInfo.CancellationTokenSource.Token); // TODO CHANGE THIS
            }

            try
            {
                jobInfo.CancellationTokenSource.Token.WaitHandle.WaitOne(SLEEP_INTERVAL);
            }
            catch (Exception)
            {

            }

            if (task.Status == TaskStatus.Faulted)
            {
                jobInfo.State = JobState.Failed;
            }

            if (task.Status == TaskStatus.Canceled)
            {
                jobInfo.State = JobState.Stopped;
            }

            if (task.Status == TaskStatus.RanToCompletion)
            {
                jobInfo.State = JobState.Ended;
            }
        }

        public void Dispose()
        {
            this._tokenSource.Cancel();
        }
    }
}