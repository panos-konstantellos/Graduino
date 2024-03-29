// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

namespace Arduino.Listener.Hosting
{
    public static class EnvironmentVariableOptionsBuilderExtensionMethods
    {
        public static StartupOptionsBuilder WithEnvironmentVariables(this StartupOptionsBuilder startupOptionsBuilder)
        {
            return startupOptionsBuilder.With(new EnvironmentVariableOptionsBuilder());
        }
    }
}