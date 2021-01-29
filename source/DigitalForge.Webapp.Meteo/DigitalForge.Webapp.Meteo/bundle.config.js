var lazypipe = require('lazypipe');
var gif = require('gulp-if');
var less = require('gulp-less');

var Bundler = (function () {

    var bundler = function () {

    };

    var stringEndsWith = function (str, suffix) {
        return str.indexOf(suffix, str.length - suffix.length) !== -1;
    };

    bundler.prototype.isLessFile = function (file) {
        return stringEndsWith(file.relative, 'less');
    };

    return bundler;

})();

var styleTransforms = lazypipe()
    .pipe(function () {

        var bundler = new Bundler();

        return gif(bundler.isLessFile, less({
            globalVars: {
                baseUrl: '"http://local.meteo.gr/"'
            }
        }));
    });


module.exports = {
    bundle: {
        modernizr: {
            scripts: [
                "./Scripts/lib/modernizr.js"
            ],
            options: {
                rev: false,
                uglify: false
            }
        },
        vendor: {
            scripts: [
                "./node_modules/jquery/dist/jquery.min.js",
                "./node_modules/lodash/lodash.min.js",
                "./node_modules/q/q.js",
                "./Scripts/lib/jquery.ba-throttle-debounce.js",
                "./node_modules/handlebars/dist/handlebars.min.js"
            ],
            styles: [],
            options: {
                rev: false,
                uglify: false
            }
        },
        digitalforge: {
            scripts: [
                "./js/Sys/DigitalForge.js",
                "./js/Sys/DigitalForge.App.js",
                "./js/Sys/DigitalForge.Configurator.js",
                "./js/Sys/DigitalForge.messagebus.js",
                "./js/Sys/messages/DigitalForge.messages.js",
                "./js/Sys/DigitalForge.Api.js",
                "./js/Sys/DigitalForge.Init.js"
            ],
            styles: [
                "./Styles/Less/mixins.less",
                "./Styles/Less/generic.less",
                "./Styles/Less/reset.less",
            ],
            options: {
                rev: false,
                uglify: false,
                transforms: {
                    styles: styleTransforms
                }
            }
        },
        meteo: {
            scripts: [
                "./node_modules/suncalc/suncalc.js",
                "./node_modules/moment/moment.js",
                "./js/Meteo/loader.js",
                "./js/Meteo/messages.js",
                "./js/Meteo/navigator.js",
                "./js/Meteo/device.js",
                "./js/Meteo/deviceDetailsPage.js",

                "./js/Meteo/helpers/mapBuilder.js",
                "./js/Meteo/helpers/Thread.js",

                "./js/Meteo/Meteo.js",
            ],
            styles: [
                "./Styles/Less/Themes/Meteo/meteo.less"
            ],
            options: {
                rev: false,
                uglify: false,
                transforms: {
                    styles: styleTransforms
                }
            }
        }
    },
};