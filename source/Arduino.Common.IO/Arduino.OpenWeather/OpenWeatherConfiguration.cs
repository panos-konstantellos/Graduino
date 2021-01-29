// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

using System.Collections.Generic;
using System.Runtime.Serialization;

using Newtonsoft.Json;

namespace Arduino.OpenWeather
{
    [DataContract]
    public class OpenWeatherConfiguration
    {
        [DataMember, JsonProperty]
        public string ApiKey { get; set; }

        private ICollection<string> _devices;

        [DataMember, JsonProperty]
        public ICollection<string> Devices
        {
            get { return this._devices ?? (this._devices = new List<string>()); }
            set { this._devices = value; }
        }
    }
}