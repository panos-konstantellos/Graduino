// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

using System;

namespace Arduino.Listener.Hosting
{
    internal sealed class StartupOptionsBuilderValidator : IStartupOptionsBuilder
    {
        void IStartupOptionsBuilder.configure(StartupOptions options)
        {
            if (options.Help)
            {
                CommandLineStartupOptionsBuilder.ShowHelp();

                Environment.Exit(0);
            }

            if (string.IsNullOrEmpty(options.PortName))
            {
                throw new Exception($"Invalid Port {options.PortName}");
            }
        }
    }
}