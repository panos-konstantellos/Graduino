// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

var DigitalForge = (function (digitalForge) {
    "use strict";

    digitalForge.Device = (function() {

        var _currentDevice = {};

        var device = function() {

        };

        device.prototype.init = function() {

            digitalForge.messageBus.subscribe(digitalForge.messages.LocationFoundMessage, 'DigitalForge.Device.init', function() {

                var location = DigitalForge.Navigator.getCurrentLocation();

                var url = !!location
                    ? DigitalForge.Api.buildUrl('device/Discover', '?latitude=' + location.latitude + '&longitude=' + location.longitude)
                    : DigitalForge.Api.buildUrl('device/Discover');

                var options = {
                    type: "GET"
                };
        
                DigitalForge.Api.call(url, options)
                    .then(function(data) {
        
                        _currentDevice = data;

                        digitalForge.messageBus.publishAsync(new digitalForge.messages.DeviceDiscoveryMessage(!!data), self);
                        
                    });
            });

            return Q.Promise(function (resolve, reject, notify) {

                resolve();

            });

        };

        device.prototype.Current = function() {

            if(!!_currentDevice)
            {
                return _.extend({}, _currentDevice);
            }

        };

        return new device();

    })();

    return digitalForge;

})(DigitalForge || {});