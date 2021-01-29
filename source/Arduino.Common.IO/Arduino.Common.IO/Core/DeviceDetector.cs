// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Arduino.Common.IO.Core
{
    public sealed class DeviceDetector : IDeviceDetector
    {
        private readonly OSPlatform _platform;

        public DeviceDetector()
        {
            this._platform = GetPlatform();
        }

        public IEnumerable<string> GetPorts()
        {
            if (this._platform == OSPlatform.Windows)
            {
                return Windows.DeviceDetector.GetPorts();
            }

            if (this._platform == OSPlatform.Linux)
            {
                return Linux.DeviceDetector.GetPorts();
            }

            throw new PlatformNotSupportedException();
        }

        private static OSPlatform GetPlatform()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return OSPlatform.Windows;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return OSPlatform.Linux;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return OSPlatform.OSX;
            }

            // if (RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
            // {
            //     return OSPlatform.FreeBSD;
            // }

            throw new PlatformNotSupportedException();
        }
    }
}