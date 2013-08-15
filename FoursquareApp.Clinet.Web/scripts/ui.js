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

    function buildAppUI(nickname) {
        var html = '<p class="text-info">' +
				nickname +
		'</p>' +
		'<button id="btn-logout" class="btn btn-danger">Logout</button><br/>' + 
                '<div class="k-content">'+
            '<div id="main-content">'+
                '<div id="tabstrip">'+
                    '<ul>'+
                        '<li class="k-state-active">'+
                            'Posts'+
                        '</li>'+
                        '<li>'+
                            'Closest locations'+
                        '</li>'+
                    '</ul>'+
                    '<div>'+
                        '<div class="places">'+
                        '</div>'+
                    '</div>'+
                    '<div>'+
                        '<div class="comments">' +
                            '<div id="all">' +
                            '</div>' +
                        '</div>'+
                    '</div>'+
                '</div>'+
            '</div>'+
        '</div>';
        return html;
    }

    function buildOpenGamesList(games) {
        var list = '<ul class="game-list open-games">';
        for (var i = 0; i < games.length; i++) {
            var game = games[i];
            list +=
				'<li data-game-id="' + game.id + '">' +
					'<a href="#" >' +
						$("<div />").html(game.title).text() +
					'</a>' +
					'<span> by ' +
						game.creatorNickname +
					'</span>' +
				'</li>';
        }
        list += "</ul>";
        return list;
    }

    function buildActiveGamesList(games) {
        var gamesList = Array.prototype.slice.call(games, 0);
        gamesList.sort(function (g1, g2) {
            if (g1.status == g2.status) {
                return g1.title > g2.title;
            }
            else {
                if (g1.status == "in-progress") {
                    return -1;
                }
            }
            return 1;
        });

        var list = '<ul class="game-list active-games">';
        for (var i = 0; i < gamesList.length; i++) {
            var game = gamesList[i];
            list +=
				'<li class="game-status-' + game.status + '" data-game-id="' + game.id + '" data-creator="' + game.creatorNickname + '">' +
					'<a href="#" class="btn-active-game">' +
						$("<div />").html(game.title).text() +
					'</a>' +
					'<span> by ' +
						game.creatorNickname +
					'</span>' +
				'</li>';
        }
        list += "</ul>";
        return list;
    }

    function buildGuessTable(guesses) {
        var tableHtml =
			'<table border="1" cellspacing="0" cellpadding="5">' +
				'<tr>' +
					'<th>Number</th>' +
					'<th>Cows</th>' +
					'<th>Bulls</th>' +
				'</tr>';
        for (var i = 0; i < guesses.length; i++) {
            var guess = guesses[i];
            tableHtml +=
				'<tr>' +
					'<td>' +
						guess.number +
					'</td>' +
					'<td>' +
						guess.cows +
					'</td>' +
					'<td>' +
						guess.bulls +
					'</td>' +
				'</tr>';
        }
        tableHtml += '</table>';
        return tableHtml;
    }

    function buildGameState(gameState) {
        var html =
			'<div id="game-state" data-game-id="' + gameState.id + '">' +
				'<h2>' + gameState.title + '</h2>' +
				'<div id="blue-guesses" class="guess-holder">' +
					'<h3>' +
						gameState.blue + '\'s gueesses' +
					'</h3>' +
					buildGuessTable(gameState.blueGuesses) +
				'</div>' +
				'<div id="red-guesses" class="guess-holder">' +
					'<h3>' +
						gameState.red + '\'s gueesses' +
					'</h3>' +
					buildGuessTable(gameState.redGuesses) +
				'</div>' +
		'</div>';
        return html;
    }

    function buildMessagesList(messages) {
        var list = '<ul class="messages-list">';
        var msg;
        for (var i = 0; i < messages.length; i += 1) {
            msg = messages[i];
            var item =
				'<li>' +
					'<a href="#" class="message-state-' + msg.state + '">' +
						msg.text +
					'</a>' +
				'</li>';
            list += item;
        }
        list += '</ul>';
        return list;
    }

    return {
        appUI: buildAppUI,
        openGamesList: buildOpenGamesList,
        loginForm: buildLoginForm,
        activeGamesList: buildActiveGamesList,
        gameState: buildGameState,
        messagesList: buildMessagesList
    }

}());