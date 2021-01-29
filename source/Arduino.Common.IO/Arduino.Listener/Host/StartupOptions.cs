// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

namespace Arduino.Listener.Hosting
{
    public sealed class StartupOptions
    {
        public string PortName { get; set; }

        public int BaudRate { get; set; }

        public bool Help { get; set; }
    }
}