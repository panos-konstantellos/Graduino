// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

using System;
using System.Collections.Generic;

namespace Arduino.OpenWeather.Threading
{
    public interface IBackgroundJobManager : IDisposable
    {
        void Start<T>(string id, string options)
            where T : IBackgroundJob;

        void Stop(string id);

        IEnumerable<JobInfo> GetActiveJobs();
    }
}