// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

using System.Linq;

using Arduino.Common.IO.Core;

namespace Arduino.Listener.Hosting
{
    internal sealed class DefaultStartupOptionsBuilder : IStartupOptionsBuilder
    {
        void IStartupOptionsBuilder.configure(StartupOptions options)
        {
            if (string.IsNullOrEmpty(options.PortName))
            {
                var ports = new DeviceDetector().GetPorts();

                if (ports.Any() && ports.Count() == 1)
                {
                    options.PortName = ports.FirstOrDefault();
                }
            }

            options.BaudRate = 9600;
        }
    }
}