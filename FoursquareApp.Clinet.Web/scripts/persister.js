/// <reference path="libs/class.js" />
/// <reference path="libs/jquery-2.0.3.intellisense.js" />
/// <reference path="libs/jquery-2.0.3.js" />
/// <reference path="http-requester.js" />


var persisters = (function () {
	var nickname = localStorage.getItem("nickname");
	var sessionKey = localStorage.getItem("sessionKey");
	function saveUserData(userData) {
		localStorage.setItem("nickname", userData.nickname);
		localStorage.setItem("sessionKey", userData.sessionKey);
		nickname = userData.nickname;
		sessionKey = userData.sessionKey;
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
			var url = this.rootUrl + "login";
			var userData = {
				username: user.username,
				authCode: CryptoJS.SHA1(user.username + user.password).toString()
			};

			httpRequester.postJson(url, userData,
				function (data) {
					saveUserData(data);
					success(data);
				}, error);
		},
		register: function (user, success, error) {
			var url = this.rootUrl + "register";
			var userData = {
				username: user.username,
				nickname: user.nickname,
				authCode: CryptoJS.SHA1(user.username + user.password).toString()
			};
			httpRequester.postJson(url, userData,
				function (data) {
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
		}
	});

	var PlacePersister = Class.create({
	    init: function (rooturl) {
	        this.rootUrl = rooturl + 'places/';
	    },
	});

	var CommentPersister = Class.create({
	    init: function (rooturl) {
	        this.rootUrl = rooturl + 'comment/';
	    },
	});

	return {
		get: function (url) {
			return new MainPersister(url);
		}
	};
}());