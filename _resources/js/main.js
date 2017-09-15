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
    gameCode = document.getElementById('base-gameCode').value.toUpperCase();
    playerName = document.getElementById('base-playerName').value;
    var ws = new WebSocket("ws://localhost:36245");
    ws.onopen = function (event) {
        console.log("Connection Established!");
        ws.send("{\"type\":\"client\",\"code\":\""+gameCode+"\",\"name\":\""+playerName+"\"}");
    };
    ws.onmessage = function (msg) {
        console.log(msg.data); //TODO remove
        let message = JSON.parse(msg.data);
        if (message.joingame) { //if this is a "client added to session" message, set display: none; on the codeEntry div
            document.getElementById('base-codeEntry').style.display = "none";
        }
        //TODO handle message
    };
    ws.onclose = function (evt) {
        console.log("Connection lost! ("+evt.code+")");
        switch (evt.code) {
            case 4001:
                document.getElementById('error-nogamecode').style.display = "initial";
                break;
            case 1006:
            default:
                document.getElementById('error-connection').style.display = "initial";
                break;
        }
    };
}