// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

using System.Runtime.Serialization;

using Newtonsoft.Json;

namespace Arduino.Listener.Models
{
    public enum UpdateStatus
    {
        Unmodified = 0,
        Inserted = 1,
        Deleted = 2,
        Modified = 3,
        InsertedOrModified = 4,
        Detached = 5
    }

    [DataContract]
    public abstract class BaseDtoObject
    {
        [DataMember, JsonProperty]
        public virtual byte[] RowVersion { get; set; }

        [DataMember, JsonProperty]
        public virtual UpdateStatus UpdateStatus { get; set; }

        [DataMember, JsonProperty]
        public virtual string GlobalId { get; set; }
    }
}