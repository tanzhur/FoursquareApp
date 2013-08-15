var ui = (function () {

    function buildLoginForm() {
        var html =
            '<div id="login-form-holder" class="row span3 offset5">' +
				'<form>' +
					'<div id="login-form">' +
						'<label for="tb-login-username">Username: </label>' +
						'<input type="text" id="tb-login-username"><br />' +
						'<label for="tb-login-password">Password: </label>' +
						'<input type="text" id="tb-login-password"><br />' +
						'<button id="btn-login" class="button btn btn-primary">Login</button>' +
					'</div>' +
					'<div id="register-form" style="display: none">' +
						'<label for="tb-register-username">Username: </label>' +
						'<input type="text" id="tb-register-username"><br />' +
						'<label for="tb-register-password">Password: </label>' +
						'<input type="text" id="tb-register-password"><br />' +
						'<button id="btn-register" class="button btn btn-primary">Register</button>' +
					'</div>' +
					'<a href="#" id="btn-show-login" class="button selected btn btn-small">Login</a>' +
					'<a href="#" id="btn-show-register" class="button btn btn-small">Register</a>' +
				'</form>' +
				'<div id="error-messages"></div>' +
            '</div>';
        return html;
    }

    function buildAppUI(nickname, lan, lat) {
        var html = '<p class="text-info">' +
				nickname +
		'</p>' +
        '<p class="text-info">' +
				lan +
		'</p>' +
        '<p class="text-info">' +
				lat +
		'</p>' +
		'<button id="btn-logout" class="btn btn-danger">Logout</button>' +
        '<button id="btn-create-place" class="btn btn-success">Create place</button>' +
                '<div class="k-content">' +
            '<div id="main-content">' +
                '<div id="tabstrip">' +
                    '<ul>' +
                        '<li class="k-state-active">' +
                            'Comments' +
                        '</li>' +
                        '<li>' +
                            'Places' +
                        '</li>' +
                    '</ul>' +
                    '<div>' +
                        '<div class="comments">' +
                            '<h3 class="text-info">All comments</h3>' +
                            '<div id="all-comments">' +
                            '</div>' +
                        '</div>' +
                    '</div>' +
                    '<div>' +
                        '<div class="places">' +
                            '<h3 class="text-info">Closest Places</h3>' +
                            '<div id="close">' +
                            '</div>' +
                            '<h3 class="text-info">All Places</h3>' +
                            '<div id="all">' +
                            '</div>' +
                            '<h3 class="text-info">My Places</h3>' +
                            '<div id="my">' +
                            '</div>' +
                        '</div>' +
                    '</div>' +
                '</div>' +
            '</div>' +
        '</div>';
        return html;
    }

    function buildAddPlaceWindow() {
        var html = '<div id="add-form" class="k-content">' +
            '<div id="window">' +
                '<p>In 1903, he founded with architects Koloman Moser and Joseph Maria Olbrich, the Wiener Werkst&auml;tte for decorative arts.</p>' +
                '<p>They aspired to the renaissance of the arts and crafts and to bring more abstract and purer forms to the designs of buildings and furniture, glass and metalwork, following the concept of total work of art</p>' +
                '</div>';
        return html;
    }

    return {
        appUI: buildAppUI,
        loginForm: buildLoginForm,
        buildPlaceAdd: buildAddPlaceWindow
    }

}());