// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

var DigitalForge = (function (digitalForge) {
    "use strict";

    digitalForge.Navigator = (function() {

        var _location;

        var getCurrentLocation = function(options) {

            var promise = Q.Promise(function (resolve, reject, notify) {
        
                if (!window.navigator.geolocation) {
        
                    resolve();
        
                    return;
        
                }
        
                window.navigator.geolocation.getCurrentPosition(function(position) {
        
                    resolve({
                        latitude: position.coords.latitude,
                        longitude: position.coords.longitude
                    });
        
                    return;
        
                }, function(error) {
        
                    resolve();
        
                    return;
        
                }, options);
        
        
            });
        
            return Q.Promise(function (resolve, reject, notify) {
            
                promise.timeout(options.timeout)
                    .then(function(data) {
        
                        resolve(data);
        
                    }, function(error) {
        
                        resolve();
        
                    });
            });
        
        };
    
        var navigator = function() {
    
        };
    
        navigator.prototype.init = function() {
    
            return Q.Promise(function (resolve, reject, notify) {
    
                getCurrentLocation({ timeout: 1000 * 60 * 60 * 24, maximumAge: 1000 * 5 })
                    .then(function(data) {

                        if(!!data) {

                            _location = data;

                        }

                        digitalForge.messageBus.publishAsync(new digitalForge.messages.LocationFoundMessage(!!data), self);

                    });
                    
                resolve();
    
            });
    
        }
    
        navigator.prototype.getCurrentLocation = function() {

            if(!!_location) {
                
                return _.extend({}, _location);

            }
    
        };
    
        return new navigator();
    
    })();

    return digitalForge;

})(DigitalForge || {});