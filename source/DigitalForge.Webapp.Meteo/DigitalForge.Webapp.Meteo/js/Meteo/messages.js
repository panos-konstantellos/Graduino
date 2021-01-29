// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

var DigitalForge = (function (digitalForge) {
    "use strict";

    // LocationFoundMessage
    var LocationFoundMessage = function(found) {
        this.found = found
    };
    LocationFoundMessage.prototype.name = 'LocationFoundMessage';

    digitalForge.messages.register(LocationFoundMessage);

    // DeviceDiscoveryMessage
    var DeviceDiscoveryMessage = function(found) {
        this.found = found
    };
    DeviceDiscoveryMessage.prototype.name = 'DeviceDiscoveryMessage';

    digitalForge.messages.register(DeviceDiscoveryMessage);

    // DeviceDiscoveryMessage
    var DetailsPageRefresh = function() {

    };
    DetailsPageRefresh.prototype.name = 'DetailsPageRefresh';

    digitalForge.messages.register(DetailsPageRefresh);
    
    return digitalForge;

})(DigitalForge || {});