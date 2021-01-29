// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Arduino.Listener
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            await Host.CreateDefaultBuilder(args)
                //.UseSystemd()
                .ConfigureServices((hostContext, services) =>
                {
                    services
                        .AddArduino()
                        .AddArduinoListener()
                        .AddClient(x => x.WithDefaults())
                        .AddHostedService<Worker>();
                })
                .Build()
                .RunAsync();
        }
    }
}