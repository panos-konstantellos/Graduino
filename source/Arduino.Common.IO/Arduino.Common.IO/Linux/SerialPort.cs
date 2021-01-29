// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

using Arduino.Common.IO.Core;

namespace Arduino.Common.IO.Linux
{
    internal class SerialPort : ISerialPort
    {
        private const int SerialBufferSize = 1024;

        private readonly CancellationTokenSource _tokenSource;

        private readonly CancellationToken _cancellationToken;

        private readonly IntPtr _readingBuffer = Marshal.AllocHGlobal(SerialBufferSize);

        private int? _fd;

        public int BaudRate { get; }

        public string PortName { get; }

        public Parity Parity { get; }

        public int DataBits { get; }

        public StopBits StopBits { get; }

        public bool IsOpen
        {
            get { return this._fd.HasValue; }
        }

        public event SerialDataReceivedEventHandler DataReceived;

        public SerialPort(string portName, int baudRate) : this(portName, baudRate, Parity.None)
        {
        }

        public SerialPort(string portName, int baudRate, Parity parity) : this(portName, baudRate, parity, 8)
        {
        }

        public SerialPort(string portName, int baudRate, Parity parity, int dataBits) : this(portName, baudRate, parity,
            dataBits, StopBits.One)
        {
        }

        public SerialPort(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
        {
            this.PortName = portName;
            this.BaudRate = baudRate;
            this.Parity = parity;
            this.DataBits = dataBits;
            this.StopBits = stopBits;

            this._tokenSource = new CancellationTokenSource();
            this._cancellationToken = this._tokenSource.Token;
        }

        public static string[] GetPortNames()
        {
            return Directory.GetFiles("/dev/", "tty*")
                .Where(port =>
                    port.StartsWith("/dev/ttyS") || port.StartsWith("/dev/ttyUSB") || port.StartsWith("/dev/ttyACM") ||
                    port.StartsWith("/dev/ttyAMA"))
                .ToArray();
        }

        public void Open()
        {
            var fd = Libc.Open(this.PortName, Libc.OpenFlags.O_RDWR | Libc.OpenFlags.O_NONBLOCK);

            if (fd == -1)
            {
                throw new Exception($"Could not open port ({this.PortName})");
            }

            this.SetBaudRate(fd);

            Task.Delay(2000);

            Task.Run(() => this.StartReading(), this._cancellationToken);

            this._fd = fd;
        }

        public void Close()
        {
            if (!this._fd.HasValue)
            {
                throw new Exception();
            }

            this._tokenSource.Cancel();
            Marshal.FreeHGlobal(this._readingBuffer);
            Libc.Close(this._fd.Value);

            this._fd = null;
        }

        public void Write(byte[] data)
        {
            if (!this._fd.HasValue)
            {
                throw new Exception();
            }

            var ptr = Marshal.AllocHGlobal(data.Length);

            Marshal.Copy(data, 0, ptr, data.Length);
            Libc.Write(this._fd.Value, ptr, data.Length);

            Marshal.FreeHGlobal(ptr);
        }

        protected virtual void OnDataReceived(byte[] data)
        {
            this.DataReceived?.Invoke(this, new SerialDataReceivedEventArgs(data));
        }

        private void SetBaudRate(int fd)
        {
            var terminalData = new byte[256];

            Libc.GetAttribute(fd, terminalData);
            Libc.SetSpeed(terminalData, this.BaudRate);
            Libc.SetAttribute(fd, 0, terminalData);
        }

        private void StartReading()
        {
            if (!this._fd.HasValue)
            {
                throw new Exception();
            }

            while (true)
            {
                this._cancellationToken.ThrowIfCancellationRequested();

                if (!File.Exists(this.PortName) || Libc.fcntl(this._fd.Value, Libc.Command.F_GETFD) == -1)
                {
                    this.Close();

                    return;
                }

                var bufferLength = Libc.Read(this._fd.Value, this._readingBuffer, SerialBufferSize);

                if (bufferLength != -1)
                {
                    var buffer = new byte[bufferLength];

                    Marshal.Copy(this._readingBuffer, buffer, 0, bufferLength);

                    this.OnDataReceived(buffer);
                }

                Task.Delay(50);
            }
        }

        #region IDisposable Support

        private bool _disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                if (this.IsOpen)
                {
                    this.Close();
                }

                this._disposedValue = true;
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        #endregion
    }
}