// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

using System;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming

namespace Arduino.Common.IO.Linux
{
    internal static class Libc
    {
        public enum Command
        {
            F_DUPFD = 0,
            F_GETFD = 1,
            F_SETFD = 2,
            F_GETFL = 3,
            F_SETFL = 4,
            F_GETOWN = 5,
            F_SETOWN = 6,
            F_GETLK = 7,
            F_SETLK = 8,
            F_SETLKW = 9
        }

        [Flags]
        public enum OpenFlags
        {
            O_RDONLY = 0,
            O_WRONLY = 1,
            O_RDWR = 2,
            O_NONBLOCK = 4
        }

        [DllImport("libc", EntryPoint = "getpid")]
        public static extern int GetProcessId();

        [DllImport("libc", EntryPoint = "tcgetattr")]
        public static extern int GetAttribute(int fd, [Out] byte[] termios_data);

        [DllImport("libc", EntryPoint = "open")]
        public static extern int Open(string pathname, OpenFlags flags);

        [DllImport("libc", EntryPoint = "close")]
        public static extern int Close(int fd);

        [DllImport("libc", EntryPoint = "read")]
        public static extern int Read(int fd, IntPtr buf, int count);

        [DllImport("libc", EntryPoint = "write")]
        public static extern int Write(int fd, IntPtr buf, int count);

        [DllImport("libc", EntryPoint = "tcsetattr")]
        public static extern int SetAttribute(int fd, int optional_actions, byte[] termios_data);

        [DllImport("libc", EntryPoint = "cfsetspeed")]
        public static extern int SetSpeed(byte[] termios_data, int speed);

        [DllImport("libc", EntryPoint = "fcntl")]
        public static extern int fcntl(int fd, Command cmd);
    }
}