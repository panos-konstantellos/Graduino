// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

using DigitalForge.Sys.Base;
using DigitalForge.Sys.Http;

namespace DigitalForge.Apps.Meteo
{
    [Register(typeof(DeviceDtoController),typeof(IDtoController<DeviceDto, IMeteoDtoRepository, IMeteoDataRepository>))]
    class DeviceDtoController : DtoController<DeviceDto, IMeteoDtoRepository, IMeteoDataRepository>, IDtoController<DeviceDto, IMeteoDtoRepository, IMeteoDataRepository>
    {
        public DeviceDtoController(IMeteoDtoRepository dtoRepository) : base(dtoRepository) { }
    }
}
