/// <reference path="libs/kendo.web.min.js" />
/// <reference path="libs/jquery-2.0.3.js" />
/// <reference path="persister.js" />
/// <reference path="libs/kendo.grid.min.js" />
/// <reference path="libs/jquery-2.0.3.intellisense.js" />
/// <reference path="ui.js" />

var controllers = (function () {
    //local check
    //var baseUrl = "http://localhost:6514/api/";
    var baseUrl = "http://foursquareapp.apphb.com/api/";

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
            var loginFormHtml = ui.loginForm();
            $(selector).html(loginFormHtml);
        },
        loadAppUI: function (selector) {
            $(selector).html(ui.appUI(
                this.persister.nickname(),
                this.persister.latitude(),
                this.persister.longtitude()));
            $("#pub-nub").append(ui.buildPubNub());
            this.loadPubNubScripts();
            this.loadPlacesTabContent(selector);
            this.loadTabScript();
            this.loadCommentsTabContent();
        },

        loadPubNubScripts: function () {
            PUBNUB.subscribe({
                channel: "userplaces-channel",
                callback: function (message) {
                    // Received a message --> print it in the page
                    $("#pn-messages ul").append('<li>' + message.User.Username + " has checked in " + message.Place.Name + " !</li>")
                    //document.getElementById('pub-nub').innerHTML += message.User.Username + " has checked in " + message.Place.Name + " !" + '\n';
                }
            });
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


        loadCommentsTabContent: function (selector) {

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

        loadPlacesTabContent: function (selector) {
            self = this;

            console.log(selector);
            console.log(ui.buildPlaceAdd());

            $(selector).append(ui.buildPlaceAdd());
            $('#add-form').hide();

            // Pop up window for place craete
            $(document).ready(function () {


                var window = $("#window"),
                    undo = $("#btn-create-place");

                undo.bind("click", function () {
                    window.data("kendoWindow").open();
                    undo.show();
                });

                if (!window.data("kendoWindow")) {
                    window.kendoWindow({
                        width: "260px",
                        actions: ["Close"],
                        title: "Add new place.",
                        close: function () {
                            undo.show();
                        }
                    });
                }
            });

            // Grid generation for places
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
                        command:
                            {
                                text: "Check Me",
                                click: function (e) {
                                    e.preventDefault();

                                    var currentPlace = this.dataItem($(e.currentTarget).closest("tr"));

                                    var messageFunction = function (message, error) {
                                        var wnd = $("#checkInMessageBox")
                                           .kendoWindow({
                                               title: "Information",
                                               modal: true,
                                               visible: false,
                                               resizable: false,
                                               width: 300
                                           }).data("kendoWindow");

                                        wnd.content(message);
                                        wnd.center().open();

                                        if (error) {
                                            console.log(error);
                                        }

                                        setTimeout(function () {
                                            wnd.center().close();
                                        }, 1500);
                                    }

                                    self.persister.user.checkIn(currentPlace.Id, function () {
                                        messageFunction("You successfully checked");
                                    }, function () {
                                       messageFunction("You successfully checked");
                                    });
                                }
                            },
                        title: "Check In",
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
                        }, {
                            title: "Comment It",
                            command: {
                                text: "Comment",
                                click: function (e) {
                                    e.preventDefault();

                                    var currentPlace = this.dataItem($(e.currentTarget).closest("tr"));
                                    
                                 
                                    var wnd = $("#checkInMessageBox")
                                        .kendoWindow({
                                            title: "Information",
                                            modal: true,
                                            visible: false,
                                            resizable: false,
                                            width: 300
                                        }).data("kendoWindow");

                                    wnd.content(ui.buildInsertCommentUI() + '<div id="hidden-data" data-place-id="'+currentPlace.Id+'"></div>');
                                    wnd.center().open();
                                }
                            }
                        }
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
            $("body").on("click", "#submit-comment", function () {
                
                
                var commentContent = $("#comment-content").val();
                var id = $("#hidden-data").data("place-id");

                var commentRegisterModel = {
                    PlaceId : id, 
                    Content: commentContent
                };

                self.persister.comment.create(commentRegisterModel, function () {
                    self.loadAppUI();
                }, function() {
                });

            });
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
                $("#pn-messages").remove();
                $("#pub-nub h4").remove();
                self.persister.user.logout(function () {
                    self.loadLoginFormUI(selector);
                }, function (err) {
                });
            });

            $("body").on("click", "#btn-crate-place", function () {
                
                var place = {
                    name: $("#new-place-name").val(),
                    longitude: $("#new-place-longtitude").val(),
                    latitude: $("#new-place-latitude").val()
                };

                console.log(place);

                self.persister.place.create(place, function () {
                    self.loadAppUI(selector);
                }, function (err) {
                    console.log(err);
                });

                return false;
            });
            
        },
    });
    return {
        get: function () {
            return new Controller();
        }
    }
}());