// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

using System.Runtime.Serialization;

using Newtonsoft.Json;

namespace Arduino.Listener.Models
{
    [DataContract]
    public class Measurement
    {
        [DataMember(Name = "temperature"), JsonProperty("temperature")]
        public decimal Temperature { get; set; }

        [DataMember(Name = "humidity"), JsonProperty("humidity")]
        public decimal Humidity { get; set; }

        [DataMember(Name = "pressure"), JsonProperty("pressure")]
        public decimal Pressure { get; set; }
    }
}