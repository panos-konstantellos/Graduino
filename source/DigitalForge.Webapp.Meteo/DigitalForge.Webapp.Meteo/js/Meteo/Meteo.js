// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

var DigitalForge = (function (digitalForge) {
    "use strict";

    digitalForge.Guid = (function () {

        var guid = function () {

        };

        guid.prototype.NewGuid = function () {

            var s4 = function () {
                return Math.floor((1 + Math.random()) * 0x10000).toString(16).substring(1);
            };

            return s4() + s4() + '-' + s4() + '-' + s4() + '-' + s4() + '-' + s4() + s4() + s4();
        };

        return new guid();

    })();

    digitalForge.map = (function () {

        var map = function (datasource, wrapper) {

            this.dataSource = datasource;
            this.wrapper = wrapper;
            this.controlId = DigitalForge.Guid.NewGuid();
            this.lock = false;

        };

        map.prototype.build = function () {

            var self = this;

            self.element = $("<div/>");
            self.element.attr('id', self.controlId);
            self.element.css('width', '100%');
            self.element.css('height', '100%');

            self.wrapper.append(self.element);

            var mapBuilderDefinition = { 
                map: {
                    center: {
                        lat: Number(_.first(this.dataSource).latitude),
                        lng: Number(_.first(this.dataSource).longtitude)
                    }
                },
                infoWindow: {
                    template: `<div class="details-wrapper">
    <div class="details">
        <a href="{{baseUrl}}devices?deviceGlobalId={{device.globalId}}">
            <div class="link-info">Click here to see more</div>
            <div class="details-block"><span class="detail-title">Updated</span><span class="details-icon icon-history"></span>{{dateTime}}</div>
            <div class="details-block"><span class="detail-title">Temperature</span><span class="details-icon icon-temperature"></span>{{device.measurement.temperature}}°C</div>
            <div class="details-block"><span class="detail-title">Humidity</span><span class="details-icon icon-humidity"></span>{{device.measurement.humidity}}%</div>
            <div class="details-block"><span class="detail-title">Pressure</span><span class="details-icon icon-atmospheric-pressure"></span>{{device.measurement.pressure}}</div>
        </a>
    </div>
</div>`
                }
            };
            
            self.mapBuilder = new DigitalForge.helpers.mapBuilder(self.element, mapBuilderDefinition);
        };

        map.prototype.render = function () {

            var self = this;

            self.map = self.mapBuilder.buildMap();

            DigitalForge.helpers.Thread.poll(function () {

                var url = DigitalForge.Api.buildUrl('device/GetLatestMeasurements');
                var options = {
                    type: "GET"
                };

                return DigitalForge.Api.call(url, options)
                    .then(function(data) {

                        self.dataSource = data;
                        self.bind();

                    });

            }, 60 * 1000);

        };

        map.prototype.bind = function () {

            var self = this;

            var definition = _.map(self.dataSource, x => {

                var measurement = _.first(x.measurements);
                
                return {
                    id: x.globalId,
                    baseUrl: DigitalForge.Configurator.configuration.Api.BaseUrl,
                    dateTime: moment.utc(measurement.dateTimeUtc).local().format('DD/MM/YYYY HH:mm'),
                    device: {
                        globalId: x.globalId,
                        latitude: Number(x.latitude),
                        longtitude: Number(x.longtitude),
                        measurement: {
                            temperature: measurement.temperature,
                            humidity: measurement.humidity,
                            pressure: measurement.pressure
                        }
                    }
                };

            });

            var markers = self.mapBuilder.buildMarkers(definition);

            if(!self.lock) {

                self.lock = true;

                var bounds = _.reduce(markers, (bounds, x) => {bounds.extend(x.marker.position);

                    return bounds;
    
                }, new google.maps.LatLngBounds());
    
                self.map.fitBounds(bounds);
    
            }

        };

        return map;

    })();

    return digitalForge;

})(DigitalForge || {});

(function () {

    $(document).ready(function () {

        var mappers = $(".map-wrapper");

        var promises = [];

        promises.push(DigitalForge.Navigator.init());
        promises.push(DigitalForge.Device.init());
        promises.push(DigitalForge.DeviceDetailsPage.init());

        _.each(mappers, function(_mapper) {

            var url = DigitalForge.Api.buildUrl('device/GetLatestMeasurements');
            var options = {
                type: "GET"
            };
    
            var promise = DigitalForge.Api.call(url, options)
                .then(function(data) {
    
                    var _map = new DigitalForge.map(data, $(_mapper));
    
                    _map.build();
                    _map.render();
                    _map.bind();

                });
    
            promises.push(promise);

        });

        Q.all(promises)
            .then(function() {

                DigitalForge.loader.hide();

            });


    });

})();

(function () {

    $(document).ready(function () {
        
        var discoveryButtons = $(".location-discover");

        discoveryButtons.addClass('disabled');
        DigitalForge.messageBus.subscribe(DigitalForge.messages.DeviceDiscoveryMessage, 'DigitalForge.Meteo.init', function() {

            if(this.found) {

                discoveryButtons.removeClass('disabled');

            } else {

                discoveryButtons.addClass('disabled');

            }

        });

        _.each(discoveryButtons, function(_button) {

            $(_button).click(function(e) {

                e.preventDefault();
                e.stopPropagation();

                if($(_button).hasClass('disabled')) {

                    return;

                }

                var currentDevice = DigitalForge.Device.Current();

                if(!currentDevice) {

                    return;

                }

                var url = DigitalForge.Configurator.configuration.Api.BaseUrl + 'devices?deviceGlobalId=' + currentDevice.globalId;

                window.location.href = url;

            });

        });

    });

})();