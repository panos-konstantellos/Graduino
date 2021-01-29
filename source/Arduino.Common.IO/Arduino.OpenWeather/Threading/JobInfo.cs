// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

using System.Threading;

namespace Arduino.OpenWeather.Threading
{
    public enum JobState
    {
        Idle = 0,

        Running = 1,

        Ended = 2,

        Stopped = 3,

        Failed = 4
    }

    public sealed class JobInfo
    {
        public string Id { get; set; }

        public string Options { get; set; }

        public IBackgroundJob Job { get; set; }

        public CancellationTokenSource CancellationTokenSource { get; set; }

        public JobState State { get; set; }
    }
}