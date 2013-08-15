/// <reference path="libs/class.js" />
/// <reference path="libs/jquery-2.0.3.intellisense.js" />
/// <reference path="libs/jquery-2.0.3.js" />
/// <reference path="http-requester.js" />

var persisters = (function () {
    var nickname = localStorage.getItem("username");
    var sessionKey = localStorage.getItem("sessionKey");
    var latitude = localStorage.getItem("lat");
    var longtitude = localStorage.getItem("lon");
    function saveUserData(userData) {
        localStorage.setItem("username", userData.UserName);
        localStorage.setItem("sessionKey", userData.SessionKey);
        localStorage.setItem("lat", userData.Latitude);
        localStorage.setItem("lon", userData.Longitude);
        latitude = userData.Latitude;
        longtitude = userData.Longitude;
        nickname = userData.UserName;
        sessionKey = userData.SessionKey;
    }
    function clearUserData() {
        localStorage.removeItem("nickname");
        localStorage.removeItem("sessionKey");
        localStorage.removeItem("lat");
        localStorage.removeItem("lon");
        longtitude = "";
        latitude = "";
        nickname = "";
        sessionKey = "";
    }

    var MainPersister = Class.create({
        init: function (rootUrl) {
            this.rootUrl = rootUrl;
            this.user = new UserPersister(this.rootUrl);
            this.place = new PlacePersister(this.rootUrl);
            this.comment = new CommentPersister(this.rootUrl);
            this.image = new ImagePersister(this.rootUrl);
        },
        isUserLoggedIn: function () {
            var isLoggedIn = nickname != null && sessionKey != null;
            return isLoggedIn;
        },
        nickname: function () {
            return nickname;
        },
        latitude: function () {
            return latitude;
        },
        longtitude: function () {
            return longtitude;
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
            console.log(user);
            var userData = {
                username: user.username,
                authCode: CryptoJS.SHA1(user.username + user.password).toString(),
                Latitude: user.latitude,
                Longitude: user.longitude
            };

            console.log(userData);
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
            var url = this.rootUrl + "create/" + sessionKey;

            httpRequester.postJson(url, data, sucess, error);
        },

        getCurrent: function (sucess, error) {
            var url = this.rootUrl + "get-current/" + sessionKey;
            httpRequester.getJson(url, sucess, error);
        },

        getCurrentService: function () {
            return url = this.rootUrl + "get-current/" + sessionKey;
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

        getAllService: function () {
            return url = this.rootUrl + "get-all/";
        },

        getCurrent: function (success, error) {
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

    var ImagePersister = Class.create({
        init: function (rooturl) {
            this.rootUrl = rooturl + 'images/';
        },
        attach: function (data, success, error) {
            var url = this.rootUrl + "attach-picture/" + sessionKey;
            httpRequester.postJson(url, data, success, error);
        },
        getAll: function (data, success, error) {
            var url = this.rootUrl + "get-all-pictures/";
            httpRequester.postJson(url, data, success, error);
        }
    });

    return {
        get: function (url) {
            return new MainPersister(url);
        }
    };
}());