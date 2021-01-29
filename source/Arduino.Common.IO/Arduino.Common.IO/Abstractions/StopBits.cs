// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

namespace Arduino.Common.IO
{
    //
    // Summary:
    //     Specifies the number of stop bits used on the System.IO.Ports.SerialPort object.
    public enum StopBits
    {
        //
        // Summary:
        //     No stop bits are used. This value is not supported by the System.IO.Ports.SerialPort.StopBits
        //     property.
        None = 0,

        //
        // Summary:
        //     One stop bit is used.
        One = 1,

        //
        // Summary:
        //     Two stop bits are used.
        Two = 2,

        //
        // Summary:
        //     1.5 stop bits are used.
        OnePointFive = 3
    }
}