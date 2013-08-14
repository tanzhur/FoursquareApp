/// <reference path="persister.js" />

$(function () {
    var persister = persisters.get("http://foursquareapp.apphb.com/api/");

    persister.user.getAll(function (data) {
        console.log(data);
    }, function (error) {
        console.log(error);
    });

    persister.comment.getAll(function (data) {
        console.log(data);
    }, function (error) {
        console.log(error);
    });
});