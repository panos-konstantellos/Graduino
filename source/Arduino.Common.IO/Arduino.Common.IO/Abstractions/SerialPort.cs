// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

using System;
using System.Runtime.InteropServices;

using Arduino.Common.IO.Core;

namespace Arduino.Common.IO
{
    public class SerialPort : ISerialPort
    {
        private readonly ISerialPort _implementation;

        public int BaudRate
        {
            get { return this._implementation.BaudRate; }
        }

        public string PortName
        {
            get { return this._implementation.PortName; }
        }

        public bool IsOpen
        {
            get { return this._implementation.IsOpen; }
        }

        public Parity Parity
        {
            get { return this._implementation.Parity; }
        }

        public int DataBits
        {
            get { return this._implementation.DataBits; }
        }

        public StopBits StopBits
        {
            get { return this._implementation.StopBits; }
        }

        public event SerialDataReceivedEventHandler DataReceived;

        public SerialPort(string portName, int baudRate)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                this._implementation = new Core.SerialPort(portName, baudRate);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
#if USE_LIBC
                this._implementation = new Arduino.Common.IO.Linux.SerialPort(portName, baudRate) as ISerialPort;
#else
                this._implementation = new Core.SerialPort(portName, baudRate);
#endif
            }
            else
            {
                throw new PlatformNotSupportedException();
            }

            this._implementation.DataReceived += this.OnDataReceived;
        }

        public static string[] GetPortNames()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return Core.SerialPort.GetPortNames();
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
#if USE_LIBC
                return Arduino.Common.IO.Linux.SerialPort.GetPortNames();
#else
                return Core.SerialPort.GetPortNames();
#endif
            }

            throw new PlatformNotSupportedException();
        }

        public void Open()
        {
            this._implementation.Open();
        }

        public void Close()
        {
            this._implementation.Close();
        }

        public void Write(byte[] data)
        {
            this._implementation.Write(data);
        }

        protected virtual void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            this.DataReceived?.Invoke(sender, e);
        }

        public void Dispose()
        {
            this._implementation.Dispose();
        }
    }
}