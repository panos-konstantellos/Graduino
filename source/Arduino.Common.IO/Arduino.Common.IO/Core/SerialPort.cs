// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

using System;
using System.Threading.Tasks;

namespace Arduino.Common.IO.Core
{
    internal sealed class SerialPort : ISerialPort
    {
        private System.IO.Ports.SerialPort _port;

        public int BaudRate { get; }

        public string PortName { get; }

        public Parity Parity { get; }

        public int DataBits { get; }

        public StopBits StopBits { get; }

        public bool IsOpen
        {
            get
            {
                if ((this._port?.IsOpen ?? false) == false)
                {
                    return false;
                }

                try
                {
                    // In Linux those holdings throw exception when cable is disconnected.
                    var flagA = this._port.DsrHolding;
                    var flagB = this._port.CtsHolding;
                }
                catch (Exception)
                {
                    return false;
                }

                return true;
            }
        }

        public event SerialDataReceivedEventHandler DataReceived;

        public SerialPort(string portName, int baudRate, Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One)
        {
            this.PortName = portName;
            this.BaudRate = baudRate;
            this.Parity = parity;
            this.DataBits = dataBits;
            this.StopBits = stopBits;
        }

        public static string[] GetPortNames()
        {
            return System.IO.Ports.SerialPort.GetPortNames();
        }

        public void Open()
        {
            var port = new System.IO.Ports.SerialPort(this.PortName, this.BaudRate,
                (System.IO.Ports.Parity) (int) this.Parity, this.DataBits,
                (System.IO.Ports.StopBits) (int) this.StopBits);

            port.Open();

            Task.Delay(2000);

            port.DataReceived += (sender, e) =>
            {
                if (!(sender is System.IO.Ports.SerialPort currentPort))
                {
                    return;
                }
                
                var data = new byte[currentPort.BytesToRead];
                currentPort.Read(data, 0, data.Length);

                this.OnDataReceived(data);
            };

            this._port = port;
        }

        public void Close()
        {
            this._port?.Close();
        }

        public void Write(byte[] data)
        {
            this._port.Write(data, 0, data.Length);
        }

        private void OnDataReceived(byte[] data)
        {
            this.DataReceived?.Invoke(this, new SerialDataReceivedEventArgs(data));
        }

        #region IDisposable Support

        private bool _disposedValue;

        private void Dispose(bool disposing)
        {
            if (this._disposedValue)
            {
                return;
            }

            if (disposing)
            {
                // TODO: dispose managed state (managed objects).
                if (this.IsOpen)
                {
                    this.Close();
                }

                this._port.Dispose();
            }

            this._disposedValue = true;
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        #endregion
    }
}