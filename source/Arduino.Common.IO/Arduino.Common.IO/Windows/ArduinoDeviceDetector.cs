// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text.RegularExpressions;

namespace Arduino.Common.IO.Windows
{
    internal sealed class DeviceDetector
    {
        private struct ArduinoProcessorInfo
        {
            public string Pid;

            public string Vid;
        }

        private static readonly IEnumerable<ArduinoProcessorInfo> AvailableArduinoProcessors = new[]
        {
            new ArduinoProcessorInfo {Pid = @"0043", Vid = @"2341"},
            new ArduinoProcessorInfo {Pid = @"0001", Vid = @"2341"},
            new ArduinoProcessorInfo {Pid = @"0043", Vid = @"2A03"},
            new ArduinoProcessorInfo {Pid = @"0243", Vid = @"2341"}
        };

        public static IEnumerable<string> GetPorts()
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher(new ManagementScope(),
                    new SelectQuery("SELECT * FROM Win32_SerialPort")))
                {
                    return searcher.Get()
                        .Cast<ManagementObject>()
                        .Where(x =>
                        {
                            var vid = Regex.Match(x["PNPDeviceID"].ToString(), @"VID_([0-9A-F]{4})",
                                RegexOptions.IgnoreCase)?.Groups[1]?.Value ?? string.Empty;
                            var pid = Regex.Match(x["PNPDeviceID"].ToString(), @"PID_([0-9A-F]{4})",
                                RegexOptions.IgnoreCase)?.Groups[1]?.Value ?? string.Empty;

                            return AvailableArduinoProcessors.Any(p => p.Vid == vid && p.Pid == pid);
                        })
                        .Select(x => x["DeviceID"].ToString());
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return new List<string>();
        }
    }
}