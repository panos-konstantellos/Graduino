// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using DigitalForge.Sys.Base;
using DigitalForge.Sys.Data;
using DigitalForge.Sys.Http;

namespace DigitalForge.Apps.Meteo
{
    public interface IDeviceController : IApiController { }

    [Register(typeof(DeviceController),typeof(IDeviceController))]
    class DeviceController : ApiControllerBase, IDeviceController
    {
        private IMeteoDataRepository repo;

        public DeviceController(IMeteoDataRepository meteoDataRepository)
        {
            this.repo = meteoDataRepository;
        }

        public override void DoInitialize(IInitializationContext initializationContext)
        {
            base.DoInitialize(initializationContext);

            this.repo.Initialize(this.initializationContext);
        }

        [HttpGet]
        public async Task<IEnumerable<DeviceDto>> Get(CancellationToken cancellationToken)
        {
            return (await this.repo.GetQuery<Device>()
                .ToListAsync(cancellationToken))
                .Select(x => new DeviceDto
                {
                    GlobalId = x.GlobalId,
                    RowVersion = x.RowVersion,

                    Code = x.Code,
                    Longtitude = x.Longtitude,
                    Latitude = x.Latitude
                });
        }

        [HttpPost]
        public async Task<string> Insert([FromBody]IEnumerable<DeviceDto> devices, CancellationToken cancellationToken)
        {
            foreach (var deviceDto in devices)
            {
                var entity = this.repo.CreateEntity<Device>(UpdateStatus.Inserted);

                entity.TenantId = 1;

                entity.Code = deviceDto.Code;
                entity.Latitude = deviceDto.Latitude;
                entity.Longtitude = deviceDto.Longtitude;
            }

            await this.repo.SaveChangesAsync(cancellationToken);

            return "Ok";
        }
    }
}
