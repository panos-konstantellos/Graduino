// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

var DigitalForge = (function (digitalForge) {
    "use strict";

    digitalForge.Api = (function () {

        var defaultOptions = {
            type: "POST",
            contentType: "application/json",
            dataType: "json",
            context: document.body,
            languageId: 1
        };

        var serverEndpoint = {};

        var api = function() {

        };

        api.prototype.init = function() {

            return Q.Promise(function (resolve, reject, notify) {

                serverEndpoint = _.extend({}, serverEndpoint, {
                    Protocol: DigitalForge.Configurator.configuration.Api.Protocol,
                    Host: DigitalForge.Configurator.configuration.Api.Host,
                    Port: DigitalForge.Configurator.configuration.Api.Port,
                    BaseUrl: DigitalForge.Configurator.configuration.Api.BaseUrl
                });

                defaultOptions = _.extend({}, defaultOptions, {
                    type: DigitalForge.Configurator.configuration.Api.DefaultMethod,
                    languageId: DigitalForge.Configurator.configuration.LanguageId
                });

                resolve();

            });

        };

        api.prototype.buildUrl = function(path, querystring) {

            var result = serverEndpoint.BaseUrl + path;

            if (querystring != null)
                result += querystring;

            return result;

        };

        api.prototype.call = function(url, options) {

            var _options = _.extend({}, defaultOptions, options || {}, {
                url: url
            });

            return Q($.ajax(_options))
                .then(function (result) {

                    return result;

                })
                .then(null, function (result) {

                    throw new Error(result.responseText);

                });

        };

        return new api();

    })();

    return digitalForge;

})(DigitalForge || {});