// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

namespace Arduino.Common.IO
{
    //
    // Summary:
    //     Specifies the parity bit for a System.IO.Ports.SerialPort object.
    public enum Parity
    {
        //
        // Summary:
        //     No parity check occurs.
        None = 0,

        //
        // Summary:
        //     Sets the parity bit so that the count of bits set is an odd number.
        Odd = 1,

        //
        // Summary:
        //     Sets the parity bit so that the count of bits set is an even number.
        Even = 2,

        //
        // Summary:
        //     Leaves the parity bit set to 1.
        Mark = 3,

        //
        // Summary:
        //     Leaves the parity bit set to 0.
        Space = 4
    }
}