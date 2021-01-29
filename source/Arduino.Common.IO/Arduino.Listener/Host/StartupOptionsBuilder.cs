// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

using System.Collections.Generic;

namespace Arduino.Listener.Hosting
{
    public sealed class StartupOptionsBuilder
    {
        private readonly List<IStartupOptionsBuilder> _startupOptionsBuilders;

        public StartupOptionsBuilder()
        {
            this._startupOptionsBuilders = new List<IStartupOptionsBuilder>();
        }

        public StartupOptionsBuilder With(IStartupOptionsBuilder builder)
        {
            this._startupOptionsBuilders.Add(builder);

            return this;
        }

        public StartupOptions Build()
        {
            var options = new StartupOptions();

            foreach (var builder in this._startupOptionsBuilders)
            {
                builder.configure(options);
            }

            return options;
        }
    }

    public static class StartupOptionsBuilderExtensionMethods
    {
        public static StartupOptionsBuilder WithCommandLineArguments(this StartupOptionsBuilder startupOptionsBuilder,
            string[] arguments)
        {
            return startupOptionsBuilder.With(new CommandLineStartupOptionsBuilder(arguments));
        }
    }
}