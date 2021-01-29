// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

using System.Collections.Generic;
using System.Runtime.Serialization;

using DigitalForge.Sys.Data;

using Newtonsoft.Json;

namespace DigitalForge.Apps.Meteo
{
    [DataContract]
    public class DeviceInverseDto : BaseDtoObject
    {
        [DataMember, JsonProperty]
        public virtual string Code { get; set; }

        [DataMember, JsonProperty]
        public virtual string Latitude { get; set; }

        [DataMember, JsonProperty]
        public virtual string Longtitude { get; set; }

        [DataMember, JsonProperty]
        public ICollection<MeasurementInverseDto> Measurements { get; set; }
    }
}
