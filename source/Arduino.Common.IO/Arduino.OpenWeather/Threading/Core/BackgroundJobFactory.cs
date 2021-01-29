// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

using System;

using Microsoft.Extensions.DependencyInjection;

namespace Arduino.OpenWeather.Threading.Core
{
    public sealed class BackgroundJobFactory : IBackgroundJobFactory
    {
        private readonly IServiceProvider _services;

        public BackgroundJobFactory(IServiceProvider services)
        {
            this._services = services;
        }

        public IBackgroundJob Create<T>(string options) where T : IBackgroundJob
        {
            var job = this._services.GetService<T>();

            if (job == null)
            {
                throw new InvalidOperationException($@"Cannot Create job {typeof(T).Name}");
            }

            job.Initialize(options);

            return job;
        }
    }
}