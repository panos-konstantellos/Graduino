// Copyright (c) 2021 Konstantellos Panagiotis, Zorbas Achileas. All rights reserved.
// Licensed under the GNU General Public License v3.0 or later. See LICENSE.md in the project root for license information.

(function(digitalForge) {
    'strict'
    
    digitalForge.messages = (function(messages) {
        
        messages.register = function(_prototype) {

            var self = this;

            if(typeof _prototype !== 'function')
                throw new Error('Invalid message type');

            if(typeof _prototype.prototype.name !== 'string' && _prototype.prototype.name != '')
                throw new Error('Invalid message type');

            if(self[_prototype.prototype.name] != null)
                throw new Error('Already registered prototype with name ' + _prototype.prototype.name);

            self[_prototype.prototype.name] = _prototype;

        };

        return messages;

    })(digitalForge.messages || {});

    digitalForge.messageBus = (function(messageBus) {

        var _messages = {};

        messageBus = function() {
        
        };

        messageBus.prototype.subscribe = function(messagefn, target, fn) {

            if(messagefn == null || digitalForge.messages[messagefn.prototype.name] == null)
                throw new Error('Message is not valid');

            if(_messages[messagefn.prototype.name] == null)
                _messages[messagefn.prototype.name] = {};
            
            var container = _messages[messagefn.prototype.name];

            if(container[target] != null)
                throw new Error('Target has already subscribed for message ' + messagefn.prototype.name);

            container[target] = fn;

        };

        messageBus.prototype.unsubscribe = function(messagefn, target) {

            if(messagefn == null || digitalForge.messages[messagefn.prototype.name] == null)
                throw new Error('Message is not valid');

            if(target == null)
                throw new Error('Invalid target');

            if(_messages[messagefn.prototype.name] != null) {

                var container = _messages[messagefn.prototype.name];
            
                if(container[target] != null)
                    delete container[target];

            }

        };

        messageBus.prototype.publish = function(message, target, envelope) {

            if(message == null || digitalForge.messages[message.name] == null)
                throw new Error('Message is not valid');

            if(_messages[message.name] != null) {

                var container = _messages[message.name];

                _.each(Object.keys(container), function(key) {

                    if(container[key] != null && typeof container[key] === 'function')
                        container[key].apply(message, [envelope, target]);

                });

            }

        };

        messageBus.prototype.publishAsync = function(message, target, envelope) {

            var self = this;

            return Q.Promise(function(resolve, reject, notify) {

                setTimeout(function() { 

                    self.publish(message, target, envelope);

                    resolve();

                }, 0);

            });

        };

        return new messageBus();

    })(digitalForge.messageBus || {});

})(DigitalForge || {});

