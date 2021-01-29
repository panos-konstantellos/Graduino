// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

var DigitalForge = (function (digitalForge) {
    "use strict";
    
    digitalForge.helpers = (function () {

        var helpers = (function (helpers) {

            helpers.mapBuilder = (function() {

                var mapBuilder = function(wrapper, definition) {
                    
                    this.wrapper = wrapper;
                    this.definition = _.merge({}, mapBuilder.prototype.defaultDefinition, definition);
                    
                    this.template = Handlebars.compile(this.definition.infoWindow.template);

                };

                mapBuilder.prototype.defaultDefinition = {
                    map: {
                        zoom: 7,
                        mapTypeId: "hybrid",
                        fullscreenControl: false,
                        mapTypeControl: false,
                        streetViewControl: false,
                        rotateControl: false,
                        center: {
                            // centers to Athens
                            lat: Number(37.948956),
                            lng: Number(23.699553)
                        }
                    },
                    infoWindow: {
                        template: ''
                    }
                }

                mapBuilder.prototype.buildMap = function() {

                    var self = this;

                    self.mapElement = new google.maps.Map(self.wrapper[0], self.definition.map);
                    
                    self.markers = [];

                    google.maps.event.addListener(self.mapElement, "click", function(event) {

                        _.each(_.values(self.markers), x => x.infoWindow.close());

                    });

                    return self.mapElement;

                }

                mapBuilder.prototype.buildMarkers = function(model) {

                    var self = this;

                    var toDelete = _.difference(
                        _.map(_.values(self.markers), x => x.id),
                        _.map(model, x => x.id)
                    );

                    _.each(toDelete, x => self.removeMarker(x));
                    _.each(model, x => self.buildMarker(x));

                    return _.values(self.markers);
                    
                };

                mapBuilder.prototype.buildMarker = function(model) {

                    var self = this;

                    var result = {
                        id: model.id,
                        marker: undefined,
                        infoWindow: undefined
                    };

                    if(!!self.markers[result.id]) {

                        result = self.markers[result.id];

                        result.marker.setMap(null);
                        result.marker = null;
                        result.infoWindow = null;

                    } else {

                        self.markers[result.id] = result;
                        
                    }

                    result.marker = new google.maps.Marker({
                        position: {
                            lat: model.device.latitude,
                            lng: model.device.longtitude
                        },
                        icon: {
                            url: model.baseUrl + '/images/marker.png',
                            size: new google.maps.Size(48, 48),
                            origin: new google.maps.Point(0, 0),
                            anchor: new google.maps.Point(24, 48)
                        },
                        map: self.mapElement
                    });

                    var content = $('<div>')
                        .addClass('infoWindow-wrapper')
                        .html(self.template(model))
                        .wrap('<div>')
                        .parent()
                        .html();

                    result.infoWindow = new google.maps.InfoWindow({ content: content });
    
                    result.marker.addListener('click', function () {

                        _.each(_.filter(_.values(self.markers), x => x.id !== result.id), x => x.infoWindow.close());

                        result.infoWindow.open(self.mapElement, result.marker);

                    });
                    
                    return result;

                };

                mapBuilder.prototype.removeMarker = function(id) {

                    var self = this;

                    var result = self.markers[id];

                    if(!result) {

                        return;

                    }

                    result.marker.setMap(null);
                    result.marker = null;
                    result.infoWindow = null;

                    delete self.markers[id];

                }

                return mapBuilder;

            })();

            return helpers;

        })(digitalForge.helpers || {});
        
        return helpers;

    })();

    return digitalForge;

})(DigitalForge || {});