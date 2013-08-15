/// <reference path="libs/kendo.web.min.js" />
/// <reference path="libs/jquery-2.0.3.js" />
/// <reference path="persister.js" />
/// <reference path="libs/kendo.grid.min.js" />
/// <reference path="libs/jquery-2.0.3.intellisense.js" />
/// <reference path="ui.js" />

var controllers = (function () {
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
            var loginFormHtml = ui.loginForm()
            $(selector).html(loginFormHtml);
        },
        loadAppUI: function (selector) {
            $(selector).html(ui.appUI(this.persister.nickname()));
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
                var user = {
                    username: $(selector).find("#tb-register-username").val(),
                    password: $(selector + " #tb-register-password").val()
                }
                self.persister.user.register(user, function () {
                    self.loadAppUI(selector);
                }, function (err) {
                    wrapper.find("#error-messages").text(err.responseJSON.Message);
                });
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