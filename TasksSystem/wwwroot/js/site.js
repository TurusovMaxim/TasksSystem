﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


(function ($) {
    "use strict";
    $(document).ready(function () {
        tinymce.init({
            selector: 'textarea'
        });

    });
})(this.jQuery);