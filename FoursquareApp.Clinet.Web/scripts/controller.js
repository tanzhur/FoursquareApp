/// <reference path="libs/kendo.web.min.js" />
/// <reference path="libs/jquery-2.0.3.js" />
/// <reference path="persister.js" />
/// <reference path="libs/kendo.grid.min.js" />
/// <reference path="libs/jquery-2.0.3.intellisense.js" />

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

    $(document).ready(function () {
        $("#grid").kendoGrid({
            dataSource: {
                transport: {
                    read: {
                        url: "http://foursquareapp.apphb.com/api/comments/get-all",
                        dataType: "json"
                    }
                },
                pageSize: 5,
            },
        });
    });
});