// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace Arduino.OpenWeather.Threading
{
    public abstract class BackgroundJob<T> : IBackgroundJob<T>
        where T : class, new()
    {
        protected T options;

        public void Initialize(string options)
        {
            this.Initialize(JsonConvert.DeserializeObject<T>(options));
        }

        public void Initialize(T options)
        {
            this.options = options;
        }

        public abstract Task ExecuteAsync(CancellationToken cancellationToken);
    }
}