#!/bin/bash
# Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
# Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

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

done