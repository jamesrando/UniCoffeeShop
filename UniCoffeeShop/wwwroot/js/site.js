// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function AddToCart(pId, itemQuantity) {
    var url = "/AddToCart";
    var cart = {
        productId: pId,
        quantity: itemQuantity
    };
    $.post(url, cart, function (data) {
    });
}