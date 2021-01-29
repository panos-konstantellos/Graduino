// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using DigitalForge.Sys.Data;

namespace DigitalForge.Apps.Meteo
{
    [Table("me_Devices")]
    public class Device : BaseDataObject
    {
        [Required, MaxLength(50)]
        public virtual string Code { get; set; }

        [Required, MaxLength(50)]
        public virtual string Latitude { get; set; }

        [Required, MaxLength(50)]
        public virtual string Longtitude { get; set; }

        [Required]
        public virtual int TenantId { get; set; }

        private ICollection<Measurement> _measurements;

        public ICollection<Measurement> Measurements
        {
            get { return this._measurements ?? (this._measurements = new List<Measurement>()); }
            set { this._measurements = value; }
        }
    }
}
