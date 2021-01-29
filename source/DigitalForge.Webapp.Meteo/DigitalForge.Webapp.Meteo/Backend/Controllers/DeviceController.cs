// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using DigitalForge.Api.Abstractions;

using WebApplication1.Http.Controllers;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public interface IDeviceController : IController
    {
        
    }

    public class DeviceController : Http.Controllers.ControllerBase, IDeviceController
    {
        private readonly IClient _client;

        public DeviceController(IClient client) : base()
        {
            this._client = client;
        }

        [HttpGet]
        public async Task<IEnumerable<Device>> GetLatestMeasurements(CancellationToken cancellationToken)
        {
            return await this._client.GetAsync<List<Device>>("api/generic/Measurement/GetLatestMeasurements", cancellationToken);
        }

        [HttpGet]
        public async Task<Device> GetLatestMeasurementsForDevice([FromQuery] string deviceGlobalId, CancellationToken cancellationToken)
        {
            return await this._client.GetAsync<Device>($"api/generic/Measurement/GetLatestMeasurementsForDevice?deviceGlobalId={deviceGlobalId}", cancellationToken);
        }

        [HttpGet]
        public async Task<Device> Discover([FromQuery] string latitude, [FromQuery] string longitude, CancellationToken cancellationToken)
        {
            var ipAddress = this.ControllerContext.HttpContext.Request.Headers.TryGetValue("X-Forwarded-For", out var _ipAddress)
                ? _ipAddress.ToString() ?? this.ControllerContext.HttpContext.Connection.RemoteIpAddress.ToString()
                : this.ControllerContext.HttpContext.Connection.RemoteIpAddress.ToString();

            var url = $"api/generic/Measurement/GetDevice?ipAddress={ipAddress}";

            if(!string.IsNullOrEmpty(latitude?.Trim() ?? string.Empty))
            {
                url = $@"{url}&latitude={latitude.Trim()}";
            }

            if(!string.IsNullOrEmpty(longitude?.Trim() ?? string.Empty))
            {
                url = $@"{url}&longitude={longitude.Trim()}";
            }

            try
            {
                return await this._client.GetAsync<Device>(url, cancellationToken);
            }
            catch (Exception)
            {
                
            }

            return null;
        }
    }
}