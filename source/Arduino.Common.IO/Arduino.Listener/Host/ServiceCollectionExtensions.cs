// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

using Arduino.Listener;
using Arduino.Listener.Core;
using Arduino.Listener.Hosting;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddArduinoListener(this IServiceCollection services)
        {
            services.AddSingleton(x =>
            {
                return new StartupOptionsBuilder()
                    //.WithCommandLineArguments(args)
                    .WithEnvironmentVariables()
                    .WithDefaults()
                    .ValidateOptions()
                    .Build();
            });

            services.AddTransient(x =>
            {
                var options = x.GetRequiredService<StartupOptions>();

                return new ArduinoListener(options.PortName, options.BaudRate);
            });

            return services;
        }
    }
}