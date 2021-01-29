// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

using System;
using System.Runtime.Serialization;

using DigitalForge.Sys.Data;

using Newtonsoft.Json;

namespace DigitalForge.Apps.Meteo
{
    [DataContract]
    public class MeasurementDto : BaseDtoObject
    {
        [DataMember, JsonProperty]
        public virtual DateTime DateTimeUtc { get; set; }

        [DataMember, JsonProperty]
        public virtual decimal Temperature { get; set; }

        [DataMember, JsonProperty]
        public virtual decimal Humidity { get; set; }

        [DataMember, JsonProperty]
        public virtual decimal Pressure { get; set; }

        [DataMember, JsonProperty]
        public virtual DeviceDto Device { get; set; }
    }
}
