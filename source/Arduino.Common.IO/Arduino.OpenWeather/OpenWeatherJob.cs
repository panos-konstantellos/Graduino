// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;

using Arduino.OpenWeather.Models;
using Arduino.OpenWeather.Threading;

using DigitalForge.Api.Abstractions;

using Microsoft.Extensions.Logging;

namespace Arduino.OpenWeather
{
    public class OpenWeatherJob : BackgroundJob<OpenWeatherJobOptions>, IBackgroundJob<OpenWeatherJobOptions>
    {
        private readonly ILogger _logger;

        private readonly IServiceProvider _services;

        public OpenWeatherJob(IServiceProvider services, ILogger<OpenWeatherJob> logger)
        {
            this._services = services;

            this._logger = logger;
        }

        public override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Device device;
            using (var client = this._services.GetService(typeof(IClient)) as IClient)
            {
                device = await client.GetAsync<Device>(
                    $@"api/data/DeviceDto/GetByGlobalId?globalId={this.options.DeviceId}", cancellationToken);
            }

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    using (var client = this._services.GetService(typeof(IClient)) as IClient)
                    using (var _client = new Client(this.options.ApiKey))
                    {
                        var weatherDto =
                            await _client.GetCurrentWeather(device.Latitude, device.Longtitude, cancellationToken);

                        var _measurments = new[]
                        {
                            new MeasurementDto
                            {
                                DateTimeUtc = DateTime.UtcNow,
                                Temperature = Convert.ToDecimal(weatherDto.Main.Temp),
                                Pressure = weatherDto.Main.Pressure,
                                Humidity = weatherDto.Main.Humidity,
                                Device = new DeviceDto
                                {
                                    GlobalId = device.GlobalId
                                },
                                UpdateStatus = UpdateStatus.Inserted,
                                GlobalId = Guid.NewGuid().ToString()
                            }
                        };

                        this._logger.LogInformation(
                            $"{DateTime.UtcNow.ToString()}: {device.GlobalId} {_measurments[0].Temperature}°C, {_measurments[0].Pressure}hpa, {_measurments[0].Humidity}%");

                        var result = await client.PostAsync<string>(@"/api/generic/Measurement/Insert", _measurments,
                            cancellationToken);
                    }
                }
                catch (Exception e)
                {
                    this._logger.LogError(e, e.Message);
                }

                await Task.Delay(30 * 1000, cancellationToken);
            }
        }
    }
}