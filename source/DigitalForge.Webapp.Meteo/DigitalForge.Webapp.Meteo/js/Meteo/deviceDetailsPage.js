// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

var DigitalForge = (function (digitalForge) {
    "use strict";

    var buildInfo = function (model) {

        var detailContainer = $('.details-wrapper .details');

        if (detailContainer.length === 0)
            return;

        var _defaultMeasurement = {
            dateTimeUtc: JSON.stringify(new Date()),
            humidity: undefined,
            pressure: undefined,
            temperature: undefined
        };
        var measurement = _.extend(_defaultMeasurement, _.last(model.measurements));

        var times = SunCalc.getTimes(new Date(), Number(model.latitude), Number(model.longtitude));

        var template = '<div class="details-block"><span class="detail-title">Code:</span> <h1>' + model.code + '</h1></div>'
            + '<div class="details-block"><span class="detail-title">Last Update:</span> <span>' + moment.utc(measurement.dateTimeUtc).local().format('DD/MM/YYYY HH:mm') + '</span></div>'
            + '<div class="details-block"><span class="detail-title">Temperature</span><span class="details-icon icon-temperature"></span><span>' + (!!measurement.temperature ? measurement.temperature + '°C': '-') + '</span></div>'
            + '<div class="details-block"><span class="detail-title">Humidity</span><span class="details-icon icon-humidity"></span><span>' + (!!measurement.humidity ? measurement.humidity + '%': '-') + '</span></div>'
            + '<div class="details-block"><span class="detail-title">Pressure</span><span class="details-icon icon-atmospheric-pressure"></span><span>' + (!!measurement.pressure ? measurement.pressure + 'hpa': '-') + '</span></div>'
            + '<div class="details-block"><span class="detail-title">Sunrise</span><span class="details-icon icon-sunrise"></span><span>' + moment(times.sunrise).format('HH:mm') + '</span></div>'
            + '<div class="details-block"><span class="detail-title">Sunset</span><span class="details-icon icon-sunset"></span><span>' + moment(times.sunset).format('HH:mm') + '</span></div>'
            ;

        detailContainer.html(template);

    };

    var buildMiniMap = function (model) {

        var element = $('.mini-map');

        if (element.length === 0)
            return;

        element.css('width', '12em');
        element.css('height', '12em');

        var map = new google.maps.Map(element[0], {
            zoom: 15,
            center: {
                lat: Number(model.latitude),
                lng: Number(model.longtitude)
            },
            mapTypeId: google.maps.MapTypeId.HYBRID,
            disableDefaultUI: true,
            zoomControl: false,
            scaleControl: false,
            streetViewControl: false,
            rotateControl: false,
            fullscreenControl: false,
            mapTypeControl: false,
            draggable: false
        });

        var image = {
            url: DigitalForge.Configurator.configuration.Api.BaseUrl + '/images/marker.png',
            size: new google.maps.Size(48, 48),
            origin: new google.maps.Point(0, 0),
            anchor: new google.maps.Point(24, 48)
        };

        var marker = new google.maps.Marker({
            position: {
                lat: Number(model.latitude),
                lng: Number(model.longtitude)
            },
            icon: image,
            map: map
        });

    };

    var buildDefinition = function (options) {

        var defaultDefinition = {
            curveType: 'function',
            legend: 'none',
            chartArea:{
                top: 10,
                left: 100,
                bottom: 50,
                right: 10
                //width:"90%",
                //height:"80%"
            }
        };

        return _.extend({}, defaultDefinition, {
            hAxis: !!options.xAxis ? options.xAxis : undefined,
            vAxis: !!options.yAxis ? options.yAxis : undefined,
            colors: !!options.colors ? options.colors : ['#dd5347']
        });

    }

    var buildDataTable = function (options, model) {

        var dt = new google.visualization.DataTable();

        _.each(options.columns, function (c) {
            dt.addColumn(c.type, c.value);
        })

        dt.addRows(_.map(model, function (row) {
            return buildDataRow(options, row);
        }));

        return dt;
    };

    var buildDataRow = function (options, row) {

        return _.map(options.columns, function (c) {

            if (c.type === 'date' || c.type === 'datetime') {

                return moment.utc(row[c.property]).local().toDate()

            }

            return row[c.property];

        });

    }

    var draw = function (data) {

        var tempDefinition = {
            xAxis: {
                format: 'd/M/yy hh:mm',
                title: 'DateTime',
                gridlines: { count: 24 }
            },
            yAxis: {
                title: 'Temperature (°C)'
            },
            colors: ['#dd5347'],
            columns: [
                {
                    type: 'datetime',
                    property: 'dateTimeUtc',
                    value: 'DateTime'
                },
                {
                    type: 'number',
                    property: 'temperature',
                    value: 'temperature'
                }
            ],
        };

        var humDefinition = {
            xAxis: {
                format: 'd/M/yy hh:mm',
                title: 'DateTime',
                gridlines: { count: 24 }
            },
            yAxis: {
                title: 'Humidity (%)'
            },
            colors: ['#dd5347'],
            columns: [
                {
                    type: 'datetime',
                    property: 'dateTimeUtc',
                    value: 'DateTime'
                },
                {
                    type: 'number',
                    property: 'humidity',
                    value: 'humidity'
                }
            ],
        };

        var pressureDeinition = {
            xAxis: {
                format: 'd/M/yy hh:mm',
                title: 'DateTime',
                gridlines: { count: 24 }
            },
            yAxis: {
                title: 'Pressure (hpa)'
            },
            colors: ['#dd5347'],
            columns: [
                {
                    type: 'datetime',
                    property: 'dateTimeUtc',
                    value: 'DateTime',
                },
                {
                    type: 'number',
                    property: 'pressure',
                    value: 'pressure'
                }
            ],
        };

        var tempDt = buildDataTable(tempDefinition, data.measurements);
        var humDt = buildDataTable(humDefinition, data.measurements);
        var pressureDt = buildDataTable(pressureDeinition, data.measurements);

        var tempChart = new google.visualization.LineChart($('.tempChart')[0]);
        var humChart = new google.visualization.LineChart($('.humChart')[0]);
        var pressChart = new google.visualization.LineChart($('.pressChart')[0]);

        tempChart.draw(tempDt, buildDefinition(tempDefinition));
        humChart.draw(humDt, buildDefinition(humDefinition));
        pressChart.draw(pressureDt, buildDefinition(pressureDeinition));
        
        var renderInfo = function (data) {

            var rows = _.map(data, function(row) {

                var template = '<div class="details-block">';

                template += '<span class="detail-title">' + row.title + '</span>';
                template += !!row.icon ? '<span class="details-icon ' + row.icon + '"></span>' : ': ';
                template += '<span>' + row.value + '</span>';
                template +='</div>';

                return template;

            });

            return _.reduce(rows, function(prev, curr) {
                return prev + curr;
            });

        };

        $('.tempInfo .details').html(renderInfo([
            { title: 'min temperature', value: _.min(_.map(data.measurements, function(x) { return x.temperature; })) + '°C', icon: 'icon-temperature' },
            { title: 'max temperature', value: _.max(_.map(data.measurements, function(x) { return x.temperature; })) + '°C', icon: 'icon-temperature' }
        ]));

        $('.humInfo .details').html(renderInfo([
            { title: 'min humidity', value: _.min(_.map(data.measurements, function(x) { return x.humidity; })) + '°%', icon: 'icon-humidity' },
            { title: 'max humidity', value: _.max(_.map(data.measurements, function(x) { return x.humidity; })) + '°%', icon: 'icon-humidity' }
        ]));

        $('.pressInfo .details').html(renderInfo([
            { title: 'min pressure', value: _.min(_.map(data.measurements, function(x) { return x.pressure; })) + 'hpa', icon: 'icon-atmospheric-pressure' },
            { title: 'max pressure', value: _.max(_.map(data.measurements, function(x) { return x.pressure; })) + 'hpa', icon: 'icon-atmospheric-pressure' }
        ]));

    }

    var parseQueryString = function(qstr) {

        var trimStartChar = function(str, char) {

            if(str.startsWith(char))
                return str.substring(1);

            return str;

        };

        return JSON.parse('{"' + decodeURI(trimStartChar(decodeURI(qstr), '?')).replace(/"/g, '\\"').replace(/&/g, '","').replace(/=/g,'":"') + '"}');

    };

    digitalForge.DeviceDetailsPage = (function() {

        var deviceDetailsPage = function() {

            this.lock = false;

        }

        deviceDetailsPage.prototype.init = function() {

            var self = this;

            if($('.devices').length === 0) {

                return Q.Promise(function (resolve, reject, notify) {

                    resolve();

                });

            }
        
            self.deviceGlobalId = parseQueryString(DigitalForge.Configurator.configuration.CurrentUrl.QueryString).deviceGlobalId;
            
            digitalForge.messageBus.subscribe(digitalForge.messages.DetailsPageRefresh, 'DigitalForge.deviceDetailsPage.init', function() {
                
                self.getData()
                    .then(function(data) {

                        $(window).trigger('resize');

                    });
            });
            
            return self.getData()
                .then(function(data) {

                    google.charts.load('current', { packages: ['corechart', 'line'] });
                    google.charts.setOnLoadCallback(function() {

                        self.render();

                        $(window).trigger('resize');

                        $(window).resize($.debounce(200, function() {
                
                            if(!self.Data) {
                                
                                return;
            
                            }
            
                            self.render();
            
                        }));
                        
                        window.setInterval(function () {

                            digitalForge.messageBus.publishAsync(new digitalForge.messages.DetailsPageRefresh(), self);
                
                        }, 1 * 60 * 1000);

                    });

                });
        
        };

        deviceDetailsPage.prototype.render = function() {
            
            var self = this;

            console.log('hi');

            buildInfo(self.Data);

            if(!self.lock) {

                self.lock = true;

                buildMiniMap(self.Data);

            }
            
            draw(self.Data);

        }

        deviceDetailsPage.prototype.getData = function() {

            var self = this;

            var url = DigitalForge.Api.buildUrl('device/GetLatestMeasurementsForDevice?deviceGlobalId=' + self.deviceGlobalId);
            var options = {
                type: "GET"
            };
    
            return DigitalForge.Api.call(url, options)
                .then(function(data) {
                    
                    self.Data = data;

                    return data;

                });
    
        };

        return new deviceDetailsPage();

    })();

    return digitalForge;

})(DigitalForge || {});