// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

var DigitalForge = (function (digitalForge) {
    "use strict";

    digitalForge.Configurator = (function () {

        var _settings = {};

        var configurator = function() {

            this.configuration = {};

        };

        configurator.prototype.init = function() {

            var self = this;

            return Q.Promise(function(resolve, reject, notify) {
                
                _settings = _.extend({}, _settings, window.DigitalForgeOptions);

                Object.defineProperty(self, "configuration",
                {
                    get: function () { return _.extend({}, _settings); }
                });

                resolve();

            });

        };

        configurator.prototype.dispose = function() {

            _settings = undefined;

        };

        return new configurator();

    })();

    return digitalForge;

})(DigitalForge || {});