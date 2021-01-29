// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

var DigitalForge = (function (digitalForge) {
    "use strict";

    digitalForge.App = (function () {

        var app = function() {

        };

        app.prototype.init = function() {

            return DigitalForge.Configurator.init()
                .then(function() {

                    return DigitalForge.Api.init();

                })
                .done(function() {

                    console.log('app initialized');

                });
            
        };

        app.prototype.run = function() {

        };

        app.prototype.dispose = function() {

        };

        return app;

    })();

    return digitalForge;

})(DigitalForge || {});