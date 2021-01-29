// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

using System;
using System.Runtime.Serialization;

using Newtonsoft.Json;

namespace Arduino.OpenWeather.Models
{
    [DataContract]
    public class Measurement
    {
        [DataMember, JsonProperty]
        public DateTime DateTimeUtc { get; set; }

        [DataMember, JsonProperty]
        public double Temperature { get; set; }

        [DataMember, JsonProperty]
        public double Humidity { get; set; }

        [DataMember, JsonProperty]
        public double Pressure { get; set; }

        [DataMember, JsonProperty]
        public byte[] RowVersion { get; set; }

        [DataMember, JsonProperty]
        public int UpdateStatus { get; set; }

        [DataMember, JsonProperty]
        public string GlobalId { get; set; }
    }
}