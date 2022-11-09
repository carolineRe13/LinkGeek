// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

window.clipboardCopy = {
    copyText: function(text) {
        return navigator.clipboard.writeText(text).then(function () {
            return true;
        })
        .catch(function (error) {
            return false;
        });
    }
};