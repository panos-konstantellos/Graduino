// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

using System.Runtime.InteropServices;
using System.Text;

using Arduino.Common.IO;

namespace Arduino.Listener.Core
{
    public class LineReceivedEventArgs
    {
        public string line { get; }

        public LineReceivedEventArgs(string line)
        {
            this.line = line;
        }
    }

    public delegate void SerialLineReceivedEventHandler(object sender, LineReceivedEventArgs e);

    public class ArduinoListener : SerialPort
    {
        private readonly StringBuilder _stringBuffer;

        private int _stateMachine;

        public const int PACKET_SIZE_DEFAULT = 1024;

        public int PacketSize { get; set; }

        public event SerialLineReceivedEventHandler LineReceived;

        public ArduinoListener(string portName, int baudRate) : base(portName, baudRate)
        {
            this._stringBuffer = new StringBuilder();
            this._stateMachine = 0;

            this.PacketSize = PACKET_SIZE_DEFAULT;
        }

        protected override void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            base.OnDataReceived(sender, e);

            var port = sender as ISerialPort;
            var newData = e.Data;

            foreach (char c in newData)
            {
                if (c == '\0')
                {
                    continue;
                }

                switch (this._stateMachine)
                {
                    case 0:

                        if (c == '\r'
                            || RuntimeInformation.ProcessArchitecture == Architecture.Arm &&
                            c == '\n' // arm7l / raspberry fix.
                            || RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && c == '\n' // ubuntu fix
                        )
                        {
                            this._stateMachine = 1;
                        }
                        else
                        {
                            while (this._stringBuffer.Length > this.PacketSize)
                            {
                                this._stringBuffer.Remove(0, 1);
                            }

                            this._stringBuffer.Append(c);
                        }

                        break;
                    case 1:

                        if (c == '\n')
                        {
                            this.LineReceived?.Invoke(this, new LineReceivedEventArgs(this._stringBuffer.ToString()));
                        }

                        this._stateMachine = 0;
                        this._stringBuffer.Clear();

                        break;
                }
            }
        }
    }
}