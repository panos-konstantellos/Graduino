// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

using DigitalForge.Sys.Base;
using DigitalForge.Sys.Data;
using DigitalForge.Sys.Http;

namespace DigitalForge.Apps.Meteo
{
    public interface IMeasurementController : IApiController { }

    [Register(typeof(MeasurementController), typeof(IMeasurementController))]
    class MeasurementController : ApiControllerBase, IMeasurementController
    {
        private IMeteoDataRepository repo;

        private IConfigurationProvider _configurationProvider;

        public MeasurementController(IMeteoDataRepository meteoDataRepository, IConfigurationProvider configurationProvider)
        {
            this.repo = meteoDataRepository;

            this._configurationProvider = configurationProvider;
        }

        public override void DoInitialize(IInitializationContext initializationContext)
        {
            base.DoInitialize(initializationContext);

            this.repo.Initialize(this.initializationContext);
        }

        [HttpGet]
        public async Task<IEnumerable<MeasurementDto>> Get(CancellationToken cancellationToken)
        {
            return (await this.repo.GetQuery<Measurement>()
                .Include(x => x.Device)
                .ToListAsync(cancellationToken))
                .Select(x => new MeasurementDto
                {
                    GlobalId = x.GlobalId,
                    RowVersion = x.RowVersion,

                    Humidity = x.Humidity,
                    Temperature = x.Temperature,
                    Pressure = x.Pressure,
                    DateTimeUtc = x.DateTimeUtc,
                    Device = new DeviceDto
                    {
                        GlobalId = x.Device.GlobalId,
                        RowVersion = x.Device.RowVersion,

                        Code = x.Device.Code,
                        Latitude = x.Device.Latitude,
                        Longtitude = x.Device.Longtitude
                    }
                });
        }

        [HttpPost]
        public async Task<string> Insert([FromBody]IEnumerable<MeasurementDto> Measurements, CancellationToken cancellationToken)
        {
            foreach (var MeasurementDto in Measurements)
            {
                var entity = this.repo.CreateEntity<Measurement>(UpdateStatus.Inserted);

                entity.TenantId = 1;

                entity.DateTimeUtc = DateTime.UtcNow;
                entity.Temperature = MeasurementDto.Temperature;
                entity.Humidity = MeasurementDto.Humidity;
                entity.Pressure = MeasurementDto.Pressure;

                entity.DeviceId = await this.repo.GetQuery<Device>()
                    .Where(x => x.GlobalId == MeasurementDto.Device.GlobalId)
                    .Select(x => x.Id)
                    .SingleAsync(cancellationToken);
            }

            await this.repo.SaveChangesAsync(cancellationToken);

            return "Ok";
        }

        [HttpGet]
        public async Task<IEnumerable<DeviceInverseDto>> GetLatestMeasurements(CancellationToken cancellationToken)
        {
            var dateFrom = DateTime.UtcNow.AddDays(-1);

            var measurements = await this.repo.GetQuery<Measurement>()
                .FromSql("SELECT * FROM me_LatestDeviceMeasurements")
                .ToListAsync(cancellationToken);

            var deviceIds = measurements.Select(x => x.DeviceId).ToList();
            var devices = await this.repo.GetQuery<Device>()
                .Where(d => deviceIds.Contains(d.Id))
                .ToListAsync(cancellationToken);

            return measurements.Select(x =>
                {
                    var device = devices.FirstOrDefault(d => d.Id == x.DeviceId);

                    return new DeviceInverseDto
                    {
                        GlobalId = device.GlobalId,
                        RowVersion = device.RowVersion,

                        Code = device.Code,
                        Latitude = device.Latitude,
                        Longtitude = device.Longtitude,
                        Measurements = new List<MeasurementInverseDto>
                        {
                            new MeasurementInverseDto
                            {
                                GlobalId = x.GlobalId,
                                RowVersion = x.RowVersion,

                                Humidity = x.Humidity,
                                Temperature = x.Temperature,
                                Pressure = x.Pressure,
                                DateTimeUtc = x.DateTimeUtc
                            }
                        }
                    };
                })
                .ToList();
        }

        [HttpGet]
        public async Task<DeviceInverseDto> GetLatestMeasurementsForDevice(string deviceGlobalId, CancellationToken cancellationToken)
        {
            var dateFrom = DateTime.UtcNow.AddDays(-1);

            var measurements = await this.repo.GetQuery<Measurement>()
                .Where(x => x.Device.GlobalId == deviceGlobalId && x.DateTimeUtc > dateFrom)
                .ToListAsync(cancellationToken);

            var device = await this.repo.GetQuery<Device>()
                .FirstOrDefaultAsync(d => d.GlobalId == deviceGlobalId, cancellationToken);

            return new DeviceInverseDto
            {
                GlobalId = device.GlobalId,
                RowVersion = device.RowVersion,

                Code = device.Code,
                Latitude = device.Latitude,
                Longtitude = device.Longtitude,

                Measurements = measurements.Select(x => new MeasurementInverseDto
                {
                    GlobalId = x.GlobalId,
                    RowVersion = x.RowVersion,

                    Humidity = x.Humidity,
                    Temperature = x.Temperature,
                    Pressure = x.Pressure,
                    DateTimeUtc = x.DateTimeUtc
                })
                .OrderBy(x => x.DateTimeUtc)
                .ToList()
            };
        }
    
        [HttpGet]
        public async Task<DeviceInverseDto> GetDevice(string ipAddress, double latitude, double longitude, CancellationToken cancellationToken)
        {
            var apiKey = this._configurationProvider.Current.GetSection("AppSettings")["ipapi.key"];

            if(latitude == default(double) || longitude == default(double))
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync($@"http://api.ipapi.com/api/{ipAddress}?access_key={apiKey}",
                        cancellationToken);

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception(await response.Content.ReadAsStringAsync());
                    }

                    dynamic result = JsonConvert.DeserializeObject<ExpandoObject>(await response.Content.ReadAsStringAsync());

                    try
                    {
                        latitude = result.latitude;
                        longitude = result.longitude;
                    }
                    catch(Exception)
                    {
                        throw new SystemException("Device not found");
                    }
                }
            }

            var devices = await this.repo.GetQuery<Device>()
                .ToListAsync(cancellationToken);

            var device = devices
                .Select(x => new 
                {
                    Distance = this.CalculateEuclideanDistance(latitude, longitude, Convert.ToDouble(x.Latitude), Convert.ToDouble(x.Longtitude)),
                    Device = x
                })
                .Where(x => x.Distance < 100)
                .OrderBy(x => x.Distance)
                .FirstOrDefault()
                .Device;

            if(device == null)
            {
                throw new SystemException("Device not found");
            }

            return new DeviceInverseDto
            {
                GlobalId = device.GlobalId,
                RowVersion = device.RowVersion,

                Code = device.Code,
                Latitude = device.Latitude,
                Longtitude = device.Longtitude,
            };
        }

        private double CalculateEuclideanDistance(double x1, double y1, double x2, double y2)
        {
            var radius = 6371.0;
            
            var lat1 = x1 * Math.PI / 180.0;
            var lat2 = x2 * Math.PI / 180.0;
            var lon1 = y1 * Math.PI / 180.0;
            var lon2 = y2 * Math.PI / 180.0;
            
            var deltaLat = lat2-lat1;
            var deltaLon = lon2-lon1;
            
            var x = deltaLon * Math.Cos((lat1+lat2)/2);
            var y = deltaLat;
            
            return radius * Math.Sqrt(x*x + y*y);
        }
    }
}
