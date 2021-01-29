// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

using Newtonsoft.Json;

namespace Arduino.OpenWeather.Threading
{
    public static class IBackgroundJobManagerExtensions
    {
        public static void Start<T, TOptions>(this IBackgroundJobManager jobManager, string id, TOptions options)
            where T : IBackgroundJob<TOptions>
            where TOptions : class, new()
        {
            jobManager.Start<T>(id, JsonConvert.SerializeObject(options));
        }
    }
}