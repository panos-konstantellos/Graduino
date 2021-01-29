// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

using System;

namespace Arduino.Listener
{
    internal static class ConsoleHelper
    {
        public static void Write(char c)
        {
            Write(c.ToString());
        }

        public static void Write(string message)
        {
            Write(message, Console.ForegroundColor);
        }

        public static void Write(char c, ConsoleColor color)
        {
            Write(c.ToString(), color);
        }

        public static void Write(string message, ConsoleColor color)
        {
            var _oldColor = Console.ForegroundColor;

            Console.ForegroundColor = color;
            Console.Write(message);

            Console.ForegroundColor = _oldColor;
        }

        public static void WriteLine(char c)
        {
            WriteLine(c.ToString());
        }

        public static void WriteLine(string message)
        {
            WriteLine(message, Console.ForegroundColor);
        }

        public static void WriteLine(char c, ConsoleColor color)
        {
            WriteLine(c.ToString(), color);
        }

        public static void WriteLine(string message, ConsoleColor color)
        {
            var _oldColor = Console.ForegroundColor;

            Console.ForegroundColor = color;
            Console.WriteLine(message);

            Console.ForegroundColor = _oldColor;
        }

        public static string CharLiteral(char c)
        {
            if (c == '\r')
            {
                return @"'\r'";
            }

            if (c == '\n')
            {
                return @"'\n'";
            }

            return string.Format("{0}", c);
        }
    }
}