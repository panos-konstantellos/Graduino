// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

(function(digitalForge) {
    'strict'
    
    // MessageBase
    var MessageBase = function() {

    };
    MessageBase.prototype.name = 'MessageBase';

    digitalForge.messages.register(MessageBase);

})(DigitalForge || {});