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
var playerName;

function DoJoinGame () {
    console.log("Button pressed!");
    gameCode = document.getElementById('base-gameCode').value.toUpperCase();
    playerName = document.getElementById('base-playerName').value;
    var ws = new WebSocket("ws://localhost:36245");
    ws.onopen = function (event) {
        console.log("Connection Established!");
        ws.send("{\"type\":\"client\",\"code\":\""+gameCode+"\",\"name\":\""+playerName+"\"");
    };
    ws.onmessage = function (msg) {
        let message = JSON.parse(msg.data);
        if (message.joingame) { //if this is a "client added to session" message, set display: none; on the codeEntry div
            document.getElementById('codeEntry').style.display = "none";
        }
        //TODO handle message
    };
    ws.onclose = function (evt) {
        console.log("Connection lost! ("+evt.code+":"+evt.reason+")");
        if (evt.code == 4001) {
            //TODO invalid game code
            return;
        } else if (evt.code == 1006) {
            //TODO connection error!
            return;
        }
        //TODO show disconnected dialog
    };
}