/// <reference path="libs/kendo.web.min.js" />
/// <reference path="libs/jquery-2.0.3.js" />
/// <reference path="persister.js" />
/// <reference path="libs/kendo.grid.min.js" />
/// <reference path="libs/jquery-2.0.3.intellisense.js" />
/// <reference path="ui.js" />

var controllers = (function () {
    //local check
    var baseUrl = "http://localhost:6514/api/";
    //var baseUrl = "http://foursquareapp.apphb.com/api/";

    var Controller = Class.create({
        init: function () {
            this.persister = persisters.get(baseUrl);
        },
        loadUI: function (selector) {
            if (this.persister.isUserLoggedIn()) {
                this.loadAppUI(selector);
            }
            else {
                this.loadLoginFormUI(selector);
            }
            this.attachUIEventHandlers(selector);
        },
        loadLoginFormUI: function (selector) {
            var loginFormHtml = ui.loginForm()
            $(selector).html(loginFormHtml);
        },
        loadAppUI: function (selector) {
            $(selector).html(ui.appUI(
                this.persister.nickname(),
                this.persister.latitude(),
                this.persister.longtitude()));
            this.loadPlacesTabContent();
            this.loadTabScript();
            this.loadCommentsTabContent();
        },

        loadTabScript: function () {
            $(document).ready(function () {
                $("#tabstrip").kendoTabStrip({
                    animation: {
                        open: {
                            effects: "fadeIn"
                        }
                    }
                });
            });
        },

        uploadImages: function () {
    
        },

        loadCommentsTabContent: function () {

            self = this;

            $(document).ready(function () {
                $("#all-comments").kendoGrid({
                    dataSource: {
                        transport: {
                            read: {
                                url: self.persister.comment.getAllSerciceUrl(),
                                dataType: "json"
                            }
                        },
                        pageSize: 10,
                    },

                    selectable: "single cell",
                    navigatable: true,
                    filterable: true,
                    sortable: true,
                    pageable: true,
                    columns: [{
                        field: "Place",
                        width: 200,
                        title: "Place"
                    }, {
                        field: "Content",
                        width: 200,
                        title: "Comment"
                    }, {
                        field: "CreationDate",
                        title: "Creation Date",
                        format: "{0:dd/MM/yyyy}"
                    }, {
                        field: "User",
                        width: 200,
                        title: "User"
                    }
                    ]
                });
            });
        },

        loadPlacesTabContent: function () {
            self = this;
            $(document).ready(function () {
                $("#close").kendoGrid({
                    dataSource: {
                        transport: {
                            read: {
                                url: self.persister.place.getClosestService(),
                                dataType: "json"
                            }
                        },
                        pageSize: 10,
                    },

                    selectable: "single cell",
                    navigatable: true,
                    filterable: true,
                    sortable: true,
                    pageable: true,
                    columns: [{
                        field: "Id",
                        width: 120,
                        title: "Id"
                    }, {
                        field: "Name",
                        width: 120,
                        title: "Place Name"
                    }, {
                        field: "Longitude",
                        width: 120,
                        title: "Longitude"
                    }, {
                        field: "Latitude",
                        width: 120,
                        title: "Latitude"
                    }, {
                        field: "Images",
                        width: 120,
                        title: "Images"
                    }, {
                        command: {
                            text: "View Details",
                            click: self.uploadImages
                        },
                        title: " ",
                        width: 120
                    }
                    ]
                });

                $(document).ready(function () {
                    $("#all").kendoGrid({
                        dataSource: {
                            transport: {
                                read: {
                                    url: self.persister.place.getAllService(),
                                    dataType: "json"
                                }
                            },
                            pageSize: 5,
                        },

                        selectable: "single cell",
                        navigatable: true,
                        filterable: true,
                        sortable: true,
                        pageable: true,
                        columns: [{
                            field: "Id",
                            width: 120,
                            title: "Id"
                        }, {
                            field: "Name",
                            width: 120,
                            title: "Place Name"
                        }, {
                            field: "Longitude",
                            width: 120,
                            title: "Longitude"
                        }, {
                            field: "Latitude",
                            width: 120,
                            title: "Latitude"
                        }, {
                            field: "Images",
                            width: 120,
                            title: "Images"
                        },
                        ]
                    }, function () {

                    });
                });

                $(document).ready(function () {
                    $("#my").kendoGrid({
                        dataSource: {
                            transport: {
                                read: {
                                    url: self.persister.place.getCurrentService(),
                                    dataType: "json"
                                }
                            },
                            pageSize: 10,
                        },

                        selectable: "single cell",
                        navigatable: true,
                        filterable: true,
                        sortable: true,
                        pageable: true,
                        columns: [{
                            field: "Id",
                            width: 120,
                            title: "Id"
                        }, {
                            field: "Name",
                            width: 120,
                            title: "Place Name"
                        }, {
                            field: "Longitude",
                            width: 120,
                            title: "Longitude"
                        }, {
                            field: "Latitude",
                            width: 120,
                            title: "Latitude"
                        }, {
                            field: "Images",
                            width: 120,
                            title: "Images"
                        },
                        ]
                    }, function () {

                    });
                });
            });
        },

        attachUIEventHandlers: function (selector) {
            var wrapper = $(selector);
            var self = this;

            wrapper.on("click", "#btn-show-login", function () {
                wrapper.find(".button.selected").removeClass("selected");
                $(this).addClass("selected");
                wrapper.find("#login-form").show();
                wrapper.find("#register-form").hide();
            });
            wrapper.on("click", "#btn-show-register", function () {
                wrapper.find(".button.selected").removeClass("selected");
                $(this).addClass("selected");
                wrapper.find("#register-form").show();
                wrapper.find("#login-form").hide();
            });

            wrapper.on("click", "#btn-login", function () {
                var user = {
                    username: $(selector + " #tb-login-username").val(),
                    password: $(selector + " #tb-login-password").val()
                }

                self.persister.user.login(user, function () {
                    self.loadAppUI(selector);
                }, function (err) {
                    wrapper.find("#error-messages").text(err.responseJSON.Message);
                });
                return false;
            });
            wrapper.on("click", "#btn-register", function () {

                if (navigator.geolocation) {
                    navigator.geolocation.getCurrentPosition(function (position) {
                        var user = {
                            username: $(selector).find("#tb-register-username").val(),
                            password: $(selector + " #tb-register-password").val(),
                            longitude: position.coords.latitude,
                            latitude: position.coords.longitude
                        }

                        self.persister.user.register(user, function () {
                            self.loadAppUI(selector);
                        }, function (err) {
                            wrapper.find("#error-messages").text(err.responseJSON.Message);
                        });

                    }, function () {
                        var user = {
                            username: $(selector).find("#tb-register-username").val(),
                            password: $(selector + " #tb-register-password").val(),
                        }

                        self.persister.user.register(user, function () {
                            self.loadAppUI(selector);
                        }, function (err) {
                            wrapper.find("#error-messages").text(err.responseJSON.Message);
                        });
                    });
                }
                else {
                    $("#btn-register").append("Geolocation is not supported by this browser.");
                }

                return false;
            });
            wrapper.on("click", "#btn-logout", function () {
                self.persister.user.logout(function () {
                    self.loadLoginFormUI(selector);
                }, function (err) {
                });
            });
        },
    });
    return {
        get: function () {
            return new Controller();
        }
    }
}());
//persister.user.getAll(function (data) {
//    console.log(data);
//}, function (error) {
//    console.log(error);
//});

//persister.comment.getAll(function (data) {
//    console.log(data);
//}, function (error) {
//    console.log(error);
//});

//$(document).ready(function () {
//    $("#grid").kendoGrid({
//        dataSource: {
//            transport: {
//                read: {
//                    url: "http://foursquareapp.apphb.com/api/comments/get-all",
//                    dataType: "json"
//                }
//            },
//            pageSize: 5,
//        },
//    });
//});