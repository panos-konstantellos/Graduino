var gulp = require('gulp');
var del = require('del');
var gwatch = require('gulp-watch');
var bundle = require('gulp-bundle-assets');

gulp.task('clean', function() {
    
    return del(['./wwwroot/assets']);

});

gulp.task('bundle', ['clean'], function() {

    return gulp.src('./bundle.config.js')
        .pipe(bundle())
        .pipe(gulp.dest('./wwwroot/assets'));

});

gulp.task('build', ['bundle'], null);
gulp.task('default', ['build'])

gulp.task('watch', function() {

    gulp.start('build');

    gwatch('./bundle.config.js', function() {

        gulp.start('build');

    });

    // gwatch('./source/**/*.{less}', function() {

    //     gulp.start('build');

    // });

});