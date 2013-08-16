var ui = (function () {

    function buildLoginForm() {
        var html =
            '<div id="login-form-holder" class="row-fluid span3 offset6">' +
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
        var html = '<div id="user-panel"><p class="text-info">Username: <strong>' +
				nickname +
		'</strong></p>' +
        '<p class="text-info">Latitude: <strong>' +
				lan +
		'</strong></p>' +
        '<p class="text-info">Longtitude: <strong>' +
				lat +
		'</strong></p></div>' +
		'<button id="btn-logout" class="btn btn-danger">Logout</button>' +
        '<button id="btn-create-place" class="btn btn-success">Create place</button>' +
                '<div class="k-content">' +
            '<div id="main-content">' +
            '<div id="checkInMessageBox"></div>' +
            '<div id="commentsInputBox"></div>' +
            '<div id="upload-box"></div>' +
            '<div id="gallery-box"></div>' +
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
                '<form>' +
					'<div id="login-form">' +
						'<label for="new-place-name">Place name: </label>' +
						'<input type="text" id="new-place-name"><br />' +
						'<label for="new-place-latitude">Latitude: </label>' +
						'<input type="text" id="new-place-latitude"><br />' +
                        '<label for="new-place-longtitude">Longtitude: </label>' +
						'<input type="text" id="new-place-longtitude"><br />' +
						'<button id="btn-crate-place" class="button btn btn-primary">Create Place</button>' +
					'</div>' +
				'</form>' +
            '</div>';
        return html;
    }

    function buildPubNubUI() {
        var html = '<h4>Leatest Check-ins</h4> \
            <div id="pn-messages"> \
                <ul> \
                </ul> \
            </div>';

        return html;
    }

    function buildCommentInput() {
        var html = "";
        html += '<h4> Comment this place </h4>' +
                '<textarea id="comment-content"></textarea>' +
                '<button id="submit-comment" class="btn btn-success">Post comment</button>';

        return html;
    }

    function buildUploadForm() {
        var html = "";
        html += '<h4>Upload image</h4>' +
                '<span> Image link : </span><input type="text" id="link-url" /><br />' +
                '<span> Image name : </span><input type="text" id="image-name" />' +
                '<button id="submit-url" class="btn btn-success">Upload image</button>';

        return html;
    }

    return {
        appUI: buildAppUI,
        loginForm: buildLoginForm,
        buildPlaceAdd: buildAddPlaceWindow,
        buildPubNub: buildPubNubUI,
        buildInsertCommentUI: buildCommentInput,
        buildUpload: buildUploadForm
    }

}());