// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

using Arduino.OpenWeather.Threading;
using Arduino.OpenWeather.Threading.Core;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterBackgroundJob<T>(this IServiceCollection services)
            where T : class, IBackgroundJob
        {
            services.AddTransient<T>();

            return services;
        }

        public static IServiceCollection AddBackgroundJobs(this IServiceCollection services)
        {
            services.AddSingleton<IBackgroundJobFactory, BackgroundJobFactory>();
            services.AddSingleton<IBackgroundJobManager, BackgroundJobManager>();

            return services;
        }
    }
}