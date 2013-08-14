/// <reference path="persister.js" />

$(function () {
    var persister = persisters.get("http://localhost:6514/api/");

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