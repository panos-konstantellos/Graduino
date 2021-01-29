// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

using System.Collections.Generic;
using System.Runtime.Serialization;

using Newtonsoft.Json;

namespace Arduino.OpenWeather.Models
{
    [DataContract]
    public class Device
    {
        [DataMember, JsonProperty]
        public string Code { get; set; }

        [DataMember, JsonProperty]
        public string Latitude { get; set; }

        [DataMember, JsonProperty]
        public string Longtitude { get; set; }

        [DataMember, JsonProperty]
        public byte[] RowVersion { get; set; }

        [DataMember, JsonProperty]
        public int UpdateStatus { get; set; }

        [DataMember, JsonProperty]
        public string GlobalId { get; set; }

        [DataMember, JsonProperty]
        public ICollection<Measurement> Measurements { get; set; }
    }
}