// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;

using Arduino.Listener.Core;
using Arduino.Listener.Models;

using DigitalForge.Api.Abstractions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Newtonsoft.Json;

namespace Arduino.Listener
{
    internal class Worker : IHostedService
    {
        private readonly IServiceProvider _services;

        private ArduinoListener _listener;

        private int _counter;

        public Worker(IServiceProvider services)
        {
            this._services = services;
        }

        async Task IHostedService.StartAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            Console.WriteLine("Application Started!");

            var t = Task.Run(async () =>
            {
                await Task.CompletedTask;

                while (true)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }

                    try
                    {
                        this._counter = 0;

                        this._listener = this._services.GetService<ArduinoListener>();

                        this._listener.LineReceived += this.SerialPort_LineReceived;

                        this._listener.Open();

                        Console.WriteLine(
                            $"Application is listening to {this._listener.PortName}:{this._listener.BaudRate}");

                        while (this._listener.IsOpen)
                        {
                            await Task.Delay(100, cancellationToken);
                        }

                        throw new Exception(
                            $"Application disconnected from {this._listener.PortName}:{this._listener.BaudRate}");
                    }
                    catch (Exception e)
                    {
                        if (this._listener?.IsOpen ?? false)
                        {
                            this._listener.Close();
                        }

                        Console.WriteLine($@"Error: {e.Message}");
                        Console.WriteLine(@"Retry in 30 seconds...");

                        await Task.Delay(30 * 1000, cancellationToken);
                    }
                }
            }, cancellationToken);
        }

        async Task IHostedService.StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            if (this._listener?.IsOpen ?? false)
            {
                this._listener.Close();
            }

            Console.WriteLine("Application ended successfully!");
        }

        private void SerialPort_LineReceived(object sender, LineReceivedEventArgs e)
        {
            this._counter++;
            if (this._counter == 29) // skip 29 lines
            {
                this._counter = 0;

                this.SendData(sender, e);
            }
        }

        private void SendData(object sender, LineReceivedEventArgs e)
        {
            try
            {
                var measurement = JsonConvert.DeserializeObject<Measurement>(e.line);
                var measurementDto = new[]
                {
                    new MeasurementDto
                    {
                        DateTimeUtc = DateTime.UtcNow,
                        Temperature = measurement.Temperature,
                        Pressure = measurement.Pressure,
                        Humidity = measurement.Humidity,
                        Device = new DeviceDto
                        {
                            GlobalId = this._services.GetService<IConfiguration>()
                                .GetSection("AppSettings")["DeviceGlobalId"]
                        },
                        UpdateStatus = UpdateStatus.Inserted,
                        GlobalId = Guid.NewGuid().ToString()
                    }
                };

                Console.WriteLine(
                    $"{DateTime.UtcNow.ToString()}: {measurement.Temperature}°C, {measurement.Pressure}hpa, {measurement.Humidity}%");

                var _tokenSource = new CancellationTokenSource();
                _tokenSource.CancelAfter(30000);

                var cancellationToken = _tokenSource.Token;

                Task.Run(() =>
                {
                    using (var client = this._services.GetService<IClient>())
                    {
                        var result = client.PostAsync<string>(@"/api/generic/Measurement/Insert", measurementDto,
                            cancellationToken).Result;
                    }
                }, cancellationToken);
            }
            catch (Exception)
            {
            }
        }
    }
}