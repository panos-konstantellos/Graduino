// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

using System;

namespace Arduino.Common.IO
{
    public sealed class SerialDataReceivedEventArgs : EventArgs
    {
        public byte[] Data { get; }

        private SerialDataReceivedEventArgs()
        {
        }

        internal SerialDataReceivedEventArgs(byte[] data)
        {
            this.Data = data;
        }
    }
}