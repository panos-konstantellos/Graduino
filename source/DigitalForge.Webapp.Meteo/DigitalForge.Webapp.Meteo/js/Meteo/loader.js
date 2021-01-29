// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

var DigitalForge = (function (digitalForge) {
    "use strict";

    digitalForge.loader = (function () {

        var loader = function () {

            this.wrapper = $('.site-loader');

        };

        loader.prototype.show = function () {

            var self = this;

            self.wrapper.show();

        };

        loader.prototype.hide = function () {

            var self = this;

            self.wrapper.hide();

        };

        return new loader();

    })();

    return digitalForge;

})(DigitalForge || {});