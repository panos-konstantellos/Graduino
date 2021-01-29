// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Arduino.Common.IO.Linux
{
    internal sealed class DeviceDetector
    {
        private static readonly List<string> ValidPorts = new List<string>
            {"/dev/ttyS", "/dev/ttyUSB", "/dev/ttyACM", "/dev/ttyAMA"};

        public static IEnumerable<string> GetPorts()
        {
            var cmd = @"#!/bin/bash

for sysdevpath in $(find /sys/bus/usb/devices/usb*/ -name dev)
do

	syspath=""${sysdevpath%/dev}"";
	devname=""$(udevadm info -q name -p $syspath)"";

	if [[ ""$devname"" == ""bus/""* ]];
	then 
		continue
	fi

	eval ""$(udevadm info -q property --export -p $syspath)""

	if [[ -z ""$ID_SERIAL"" ]];
	then 
		continue
	fi

	echo ""/dev/$devname,$ID_SERIAL""

done".Replace("\r\n", "\n");

            return ShellRunner.ExecuteCommand(cmd)
                .Split(Environment.NewLine)
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x =>
                {
                    var arguments = x.Split(',');

                    return new
                    {
                        Path = arguments[0],
                        SerialId = arguments[1]
                    };
                })
                .Where(x => x.SerialId.Contains("arduino") &&
                            ValidPorts.Any(y => x.Path.StartsWith(y, StringComparison.OrdinalIgnoreCase)))
                .Select(x => x.Path)
                .ToList();
        }
    }
}