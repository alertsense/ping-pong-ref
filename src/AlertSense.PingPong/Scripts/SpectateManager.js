(function (spectateManager, $, undefined) {
    var $gameState, $gameId, $side1Score, $side2Score, $infoTable, $winModal, $winSide;

    // In .NET, A single tick represents one hundred nanoseconds or one ten-millionth of a second.
    // There are 10,000 ticks in a millisecond, or 10 million ticks in a second. See
    // https://msdn.microsoft.com/en-us/library/system.datetime.ticks.aspx for more details.
    var ticksPerMillisecond = 10000;

    spectateManager.init = function () {
        $gameState = $('#game-state');
        $gameId = $('#game-id');
        $side1Score = $('#side-one-score');
        $side2Score = $('#side-two-score');
        $infoTable = $('.game-info-table');
        $winModal = $('#win-modal');
        $winSide = $('#win-side');

        var spectator = $.connection.spectator;
        $.extend(spectator.client, {
            update: spectateManager.update
        });
        // Start listening for events from the server
        $.connection.hub.start();
    };

    spectateManager.update = function (model) {
        updateGameState(model);
        updateScore(model);
        updateInfoTable(model);

        if (model.GameState == 1) {
            // Game Over, Congratulate Winner
            $winSide.html(model.Players[0].Score > model.Players[1].Score ? "One" : "Two");
            $winModal.modal('show');
            // Stop listening for events from the server
            $.connection.hub.stop();
        }
    };

    var updateGameState = function (model) {
        $gameState.html(model.GameStateString);
        $gameId.html(model.Id);
    };

    var updateScore = function (model) {
        $side1Score.html(model.Players[0].Score);
        $side2Score.html(model.Players[1].Score);

        if (model.Striker == 0) {
            $side1Score.addClass("serving");
            $side2Score.removeClass("serving");
        } else {
            $side1Score.removeClass("serving");
            $side2Score.addClass("serving");
        }
    };

    var updateInfoTable = function (model) {
        $infoTable.find('tbody tr').remove();

        var createdMilliseconds = model.Created / ticksPerMillisecond;
        for (var i = 0; i < model.Points.length; i++) {
            var point = model.Points[i];

            var pointMilliseconds = point.Ticks / ticksPerMillisecond;

            var sideOneInfo = point.SideToAward == 0 ? "Point Awarded" : "";
            var time = ((pointMilliseconds - createdMilliseconds) / 1000).toFixed(3);
            var sideTwoInfo = point.SideToAward == 1 ? "Point Awarded" : "";

            $infoTable.find('tbody').prepend('<tr><td>' + sideOneInfo + '</td><td>' + time + '</td><td>' + sideTwoInfo + '</td></tr>');
        }
    };
})(window.spectateManager = window.spectateManager || {}, jQuery);