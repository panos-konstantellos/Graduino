// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

using Arduino.Listener.Hosting;

namespace Arduino.Listener
{
    public static class DefaultStartupOptionsBuilderExtensionMethods
    {
        public static StartupOptionsBuilder WithDefaults(this StartupOptionsBuilder startupOptionsBuilder)
        {
            return startupOptionsBuilder.With(new DefaultStartupOptionsBuilder());
        }
    }
}