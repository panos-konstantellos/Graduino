// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

using System;

using Arduino.Common.IO.Core;

namespace Arduino.Common.IO
{
    public interface ISerialPort : IDisposable
    {
        int BaudRate { get; }

        string PortName { get; }

        bool IsOpen { get; }

        Parity Parity { get; }

        int DataBits { get; }

        StopBits StopBits { get; }

        void Open();

        event SerialDataReceivedEventHandler DataReceived;

        void Close();

        void Write(byte[] data);
    }
}