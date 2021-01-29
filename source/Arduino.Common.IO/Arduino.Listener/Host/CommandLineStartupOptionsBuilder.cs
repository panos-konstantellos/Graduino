// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

using System;
using System.Linq;
using System.Reflection;

using Mono.Options;

namespace Arduino.Listener.Hosting
{
    internal sealed class CommandLineStartupOptionsBuilder : IStartupOptionsBuilder
    {
        private readonly string[] _options;

        public CommandLineStartupOptionsBuilder(string[] options)
        {
            this._options = options;
        }

        void IStartupOptionsBuilder.configure(StartupOptions options)
        {
            try
            {
                var rest = getOptionSet(options).Parse(this._options);
            }
            catch (OptionException)
            {
                Console.WriteLine("Try `--help' for more information.");
            }
        }

        public static void ShowHelp()
        {
            getOptionSet(new StartupOptions()).WriteOptionDescriptions(Console.Out);
        }

        private static OptionSet getOptionSet(StartupOptions options)
        {
            var result = new OptionSet();

            var properties = options.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .ToList();

            foreach (var prop in properties)
            {
                var propName = prop.Name.ToLower();

                if (propName.Contains("help")) // enhance this to use attributes.
                {
                    result.Add($"{propName}", x => prop.SetValue(options, x));

                    continue;
                }

                result.Add($"{propName}=", $"the {propName} to use",
                    x => prop.SetValue(options, Convert.ChangeType(x, prop.PropertyType)));
            }

            return result;
        }
    }
}