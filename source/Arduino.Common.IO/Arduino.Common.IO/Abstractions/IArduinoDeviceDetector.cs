// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

using System.Collections.Generic;

namespace Arduino.Common.IO
{
    public interface IDeviceDetector
    {
        IEnumerable<string> GetPorts();
    }
}