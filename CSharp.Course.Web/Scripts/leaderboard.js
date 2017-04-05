﻿(function () {
    function populateLeaderboard(board) {
        var entityMap = {
            '&': '&amp;',
            '<': '&lt;',
            '>': '&gt;',
            '"': '&quot;',
            "'": '&#39;',
            '/': '&#x2F;',
            '`': '&#x60;',
            '=': '&#x3D;'
        };

        for (var index = 1; index <= 10; index++) {
            var tableRow = '<tr><td>' + index + '</td>';

            if (board.length > index - 1) {
                var item = board[index - 1];
                tableRow += '<td>' + item.Username.replace(/[&<>"'`=\/]/g, function (s) {
                    return entityMap[s];
                }) + '</td>';
                tableRow += '<td>' + item.Passed + '</td>';
                tableRow += '<td>' + item.Skipped + '</td>';
                tableRow += '<td>' + item.Failed + '</td>';
            }

            tableRow += '</tr>';
            console.log(tableRow);
            $('#leaderboard tr:last').after(tableRow);
        }
    }
    $.ajax('/api/leaderboard', {
        dataType: 'json',
        success: function (data, status) {
            populateLeaderboard(data);

            var leaderboardHubProxy = $.connection.leaderboardHub;
            leaderboardHubProxy.client.leaderboardUpdate = function (board) {
                console.log(JSON.stringify(board));
                $('#leaderboard ').find('tr:gt(0)').remove();
                populateLeaderboard(board);
            };
            $.connection.hub.start()
                .done(function () { console.log('Now connected, connection ID=' + $.connection.hub.id); })
                .fail(function () { console.log('Could not Connect!'); });
        },
        error: function () {
            $('#errorMessage').show();
        }
    });





})();