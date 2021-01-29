// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

using DigitalForge.Sys.Base;
using DigitalForge.Sys.Data;

namespace DigitalForge.Apps.Meteo
{
    public interface IMeteoDataRepository : IDataRepository
    {
        
    }

    [Register(typeof(MeteoDataRepository), typeof(IMeteoDataRepository))]
    class MeteoDataRepository : DataRepository, IMeteoDataRepository
    {

    }
}
