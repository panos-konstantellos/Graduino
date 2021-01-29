// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

using System;
using System.Linq;
using System.Reflection;

namespace Arduino.Listener.Hosting
{
    internal sealed class EnvironmentVariableOptionsBuilder : IStartupOptionsBuilder
    {
        void IStartupOptionsBuilder.configure(StartupOptions options)
        {
            var properties = options.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .ToList();

            foreach (var prop in properties)
            {
                var value = Environment.GetEnvironmentVariable($"METEO_{prop.Name}".ToUpper());

                if (!string.IsNullOrEmpty(value))
                {
                    prop.SetValue(options, Convert.ChangeType(value, prop.PropertyType));
                }
            }
        }
    }
}