/// <reference path="libs/class.js" />
/// <reference path="libs/jquery-2.0.3.intellisense.js" />
/// <reference path="libs/jquery-2.0.3.js" />
/// <reference path="http-requester.js" />


var persisters = (function () {
    var nickname = localStorage.getItem("username");
    var sessionKey = localStorage.getItem("sessionKey");
    function saveUserData(userData) {
        localStorage.setItem("username", userData.UserName);
        localStorage.setItem("sessionKey", userData.SessionKey);
        nickname = userData.UserName;
        sessionKey = userData.SessionKey;
    }
    function clearUserData() {
        localStorage.removeItem("nickname");
        localStorage.removeItem("sessionKey");
        nickname = "";
        sessionKey = "";
    }

    var MainPersister = Class.create({
        init: function (rootUrl) {
            this.rootUrl = rootUrl;
            this.user = new UserPersister(this.rootUrl);
            this.place = new PlacePersister(this.rootUrl);
            this.comment = new CommentPersister(this.rootUrl);
        },
        isUserLoggedIn: function () {
            var isLoggedIn = nickname != null && sessionKey != null;
            return isLoggedIn;
        },
        nickname: function () {
            return nickname;
        }
    });
    var UserPersister = Class.create({
        init: function (rootUrl) {
            this.rootUrl = rootUrl + "users/";
        },
        login: function (user, success, error) {
            var url = this.rootUrl + "login/";
            var userData = {
                Username: user.username,
                AuthCode: CryptoJS.SHA1(user.username + user.password).toString()
            };

            httpRequester.postJson(url, userData,
				function (data) {
				    saveUserData(data);
				    success(data);
				}, error);
        },
        register: function (user, success, error) {
            var url = this.rootUrl + "register/";
            var userData = {
                username: user.username,
                authCode: CryptoJS.SHA1(user.username + user.password).toString()
            };
            httpRequester.postJson(url, userData,
				function (data) {
				    console.log(data);
				    saveUserData(data);
				    success(data);
				}, error);
        },
        logout: function (success, error) {
            var url = this.rootUrl + "logout/" + sessionKey;
            httpRequester.getJson(url, function (data) {
                clearUserData();
                success(data);
            }, error)
        },
        getAll: function (success, error) {
            var url = this.rootUrl + "get-all/";
            httpRequester.getJson(url, success, error);
        },

        checkIn: function (data, success, error) {
            var url = this.rootUrl + "check-in/" + sessionKey;
            httpRequester.postJson(url, data, success, error);
        }
    });

    var PlacePersister = Class.create({
        init: function (rooturl) {
            this.rootUrl = rooturl + 'places/';
        },
        create: function (data, sucess, error) {
            var url = this.rooturl + "create/";

            httpRequester.postJson(url, data, sucess, error);
        },

        getClosest: function (success, error) {
            var url = this.rootUrl + "get-closest/" + sessionKey;
            httpRequester.getJson(url, success, error);
        },
        getClosestService: function (success, error) {
            var url = this.rootUrl + "get-closest/" + sessionKey;
            return url;
        },
        getAll: function (success, error) {
            var url = this.rootUrl + "get-all/";
            httpRequester.getJson(url, sucess, error);
        },

        getAll: function (success, error) {
            var url = this.rootUrl + "get-current/" + sessionKey;
            httpRequester.getJson(url, sucess, error);
        }
    });

    var CommentPersister = Class.create({
        init: function (rooturl) {
            this.rootUrl = rooturl + 'comments/';
        },
        create: function (data, success, error) {
            var url = this.rootUrl + "create/" + sessionKey;
            httpRequester.postJson(url, data, success, error);
        },

        getAll: function (success, error) {
            var url = this.rootUrl + "get-all/"
            httpRequester.getJson(url, success, error);
        },

        getAllSerciceUrl: function () {
            var url = this.rootUrl + "get-all/";
            return url;
        }
    });

    return {
        get: function (url) {
            return new MainPersister(url);
        }
    };
}());