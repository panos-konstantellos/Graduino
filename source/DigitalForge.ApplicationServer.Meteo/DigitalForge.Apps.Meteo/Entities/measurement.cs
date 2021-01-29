// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using DigitalForge.Sys.Data;

namespace DigitalForge.Apps.Meteo
{
    [Table("me_Measurements")]
    public class Measurement : BaseDataObject
    {
        [Required]
        public virtual DateTime DateTimeUtc { get; set; }

        [Required]
        public virtual decimal Temperature { get; set; }

        [Required]
        public virtual decimal Humidity { get; set; }

        [Required]
        public virtual decimal Pressure { get; set; }

        [Required]
        public virtual int DeviceId { get; set; }

        [ForeignKey("DeviceId")]
        public virtual Device Device { get; set; }

        [Required]
        public virtual int TenantId { get; set; }
    }
}
