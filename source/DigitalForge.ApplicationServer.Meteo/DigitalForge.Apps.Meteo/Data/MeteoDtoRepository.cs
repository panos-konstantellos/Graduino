// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

using DigitalForge.Sys.Base;
using DigitalForge.Sys.Data;

namespace DigitalForge.Apps.Meteo
{
    public interface IMeteoDtoRepository : IDtoRepository<IMeteoDataRepository>
    {
        
    }

    [Register(typeof(MeteoDtoRepository), typeof(IMeteoDtoRepository))]
    class MeteoDtoRepository : DtoRepository<IMeteoDataRepository>, IMeteoDtoRepository
    {
        public MeteoDtoRepository(IMeteoDataRepository dataRepository, IDataMapper dataMapper) : base(dataRepository, dataMapper)
        {
        }

        protected override void DoRegisterMappings()
        {
            this.DataMapper.RegisterMapping<Device, DeviceDto>();
            this.DataMapper.RegisterMapping<Measurement, MeasurementDto>();
            this.DataMapper.RegisterMapping<Device, DeviceInverseDto>();
            this.DataMapper.RegisterMapping<Measurement, MeasurementInverseDto>();
        }

        protected override BaseDataObject MapToDataObject<TEntityDto>(TEntityDto entityDto, BaseDataObject entity)
        {
            var result = base.MapToDataObject(entityDto, entity);

            if (result.GetType() == typeof(Device))
            {
                ((Device) result).TenantId = 1;
            }

            return result;
        }
    }
}
