// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

using Arduino.Common.IO;
using Arduino.Common.IO.Core;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddArduino(this IServiceCollection services)
        {
            services.AddTransient<IDeviceDetector, DeviceDetector>();

            return services;
        }
    }
}