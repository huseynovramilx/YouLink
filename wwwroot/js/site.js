// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
window.onload = function () {
    let culture = $('#culture');
    let dropdown = $('.dropdown-item');
    let form = $('#selectLanguage');
    dropdown.click(function () {
        culture.val($(this).data('value'));
        form.submit();
    });
};