// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

using System.Linq;

using DigitalForge.Sys.Base;
using DigitalForge.Sys.Http;

using Microsoft.AspNetCore.Mvc;

namespace DigitalForge.Apps.Meteo.Http.BreezeControllers
{
    public interface IMeteoBreezeController : IBreezeController
    {
    }

    //[RegisterMetadata(typeof(MeteoBreezeController))]
    [Register(typeof(MeteoBreezeController), typeof(IMeteoBreezeController))]
    public class MeteoBreezeController : BreezeController, IMeteoBreezeController
    {
        [HttpGet, RegisterResource(typeof(Device), true)]
        public IQueryable<Device> Devices()
        {
            return this.GetQuery<Device>();
        }

        [HttpGet, RegisterResource(typeof(Measurement), true)]
        public IQueryable<Measurement> Measurements()
        {
            return this.GetQuery<Measurement>();
        }
    }
}
