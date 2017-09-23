window.onload = function () {
    //process hash in the URL, link from game QR code
    if (window.location.hash != '') {
        document.getElementById('base-gameCode').value = window.location.hash.split('#')[1];
    }
    history.pushState(null, "Brawl o' Chums", location.href.split("#")[0].toUpperCase());
    //if the user has entered a name in the past, fill it in
    if (getCookie('boc-playername') !== null) {
        document.getElementById('base-playerName').value = getCookie('boc-playername');
    }
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
var identifier;

function DoJoinGame () { //TODO spinny animation to show work
    document.getElementById('base-submit').isDisabled = true;
    gameCode = document.getElementById('base-gameCode').value.toUpperCase();
    if (gameCode === "") {
        document.getElementById('error-nogamecode').style.display = "initial";
        return;
    }
    playerName = document.getElementById('base-playerName').value;
    if (playerName === "") {
        //TODO handle invalid or empty player name
        return;
    }
    document.cookie = "boc-playername="+playerName+"; expires=33071673599000; path=/"; //set the player name to a cookie for later retrieval
    var ws = new WebSocket("ws://localhost:36245");
    ws.onopen = function (event) {
        console.log("Connection Established!");
        //send type, code, name, and identifier (if we have one) to the server
        ws.send("{\"type\":\"client\",\"code\":\""+gameCode+"\",\"name\":\""+playerName+"\""+(getCookie('boc-identifier') !== null ? ",\"identifier\":\""+getCookie('boc-indentifier')+"\"" : "")+"}");
    };
    ws.onmessage = function (msg) {
        console.log(msg.data); //TODO remove
        let message = JSON.parse(msg.data);
        if (message.joingame) { //if this is a "client added to session" message, switch to the logo screen until a update command received
            var now = new Date();
            now.setTime(now.getTime()+1800*1000);
            identifier = message.identifier;
            document.cookie = "boc-identifier="+message.identifier+"; expires="+now.toUTCString()+"; path=/"; //store the identifier the server gave us for 30 mins
            document.getElementById('base-codeEntry').style.display = "none";
            document.getElementById('other-lobby').style.display = "initial";
            return;
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
            case 4002:
            default:
                document.getElementById('error-connection').style.display = "initial";
                break;
        }
    };
}