// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;

using Arduino.OpenWeather.Threading;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Arduino.OpenWeather
{
    public class Worker : BackgroundService
    {
        private readonly ILogger _logger;

        private readonly IServiceProvider _services;

        public Worker(IServiceProvider services, ILogger<Worker> logger)
        {
            this._services = services;
            this._logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            this._logger.LogInformation($@"Worker running at: {DateTimeOffset.Now}");

            var jobManager = this._services.GetService<IBackgroundJobManager>();

            var config = this._services.GetService<IConfiguration>()
                .GetSection("OpenWeather")
                .Get<OpenWeatherConfiguration>();

            foreach (var device in config.Devices)
            {
                jobManager.Start<OpenWeatherJob, OpenWeatherJobOptions>(device, new OpenWeatherJobOptions
                {
                    ApiKey = config.ApiKey,
                    DeviceId = device
                });
            }

            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(100, cancellationToken);
            }

            jobManager.Dispose();
        }
    }
}