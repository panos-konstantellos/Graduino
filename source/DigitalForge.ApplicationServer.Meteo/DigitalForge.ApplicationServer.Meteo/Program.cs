// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

using DigitalForge.Sys.Base;
using DigitalForge.Sys.ModuleManager;

namespace DigitalForge.ApplicationServer.Meteo
{
    class Program
    {
        static void Main(string[] args)
        {
            ModuleManager.Current.Initialize();
            Globals.Instance.ClassFactory = ModuleManager.Current.ClassFactory;

            Globals.Instance.ClassFactory.CreateObject<DigitalForge.Sys.Http.IWebHostBuilder>().Host(args);
        }
    }
}
