window.onload = function () {
    if (window.location.hash != '') {
        document.getElementById('base-gameCode').value = window.location.hash.split('#')[1];
    }
    history.pushState(null, "Brawl o' Chums", location.href.split("#")[0].toUpperCase());
};
window.onhashchange = function () {
    if (window.location.hash != '') {
        document.getElementById('base-gameCode').value = window.location.hash.split('#')[1];
    }
    history.pushState(null, "Brawl o' Chums", location.href.split("#")[0].toUpperCase());
};

//TODO player name control (<=20 chars, emoji+utf8)

var gameCode;

function DoJoinGame () {
    gameCode = document.getElementById('base-gameCode').value.toUpperCase();
    var ws = new WebSocket("ws://localhost:36245");
    ws.onopen = function (event) {
        ws.send("{\"type\":\"client\",\"code\":\""+gameCode+"\""); //TODO include name in handshake
    };
    ws.onmessage = function (msg) {
        //TODO if this is a "client added to session" message, set display: none; on the codeEntry div
        //TODO handle message
    };
    ws.onclose = function (evt) {
        console.log("Connection lost! ("+evt.code+":"+evt.reason+")");
        if (evt.code == 4001) {
            //TODO invalid game code
            return;
        }
        //TODO show disconnected dialog
    };
}