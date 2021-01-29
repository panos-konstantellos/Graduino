// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

var DigitalForge = (function (digitalForge) {
    "use strict";
    
    digitalForge.helpers = (function (helpers) {

        helpers.Thread = (function() {

            var thread = function() {

            };

            thread.prototype.sleep = async function(interval) {

                await new Promise(x => setTimeout(x, interval));

            };

            thread.prototype.poll = function(fn, timeout) {

                var self = this;

                setTimeout(() => {

                    var result = fn();

                    if(Q.isPromise(result)) {

                        result.then(function() {

                            self.poll(fn, timeout);

                        }).then(null, function(error) {

                            console.error(error);

                            self.poll(fn, timeout);

                        });

                    } else {

                        self.poll(fn, timeout);

                    }

                    
                }, timeout);

            };

            return new thread();

        })();
            
        return helpers;

    })(digitalForge.helpers || {});
        
    return digitalForge;

})(DigitalForge || {});