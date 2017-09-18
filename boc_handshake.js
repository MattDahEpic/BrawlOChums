const s = require('./boc_struct.js');
const randomstring = require('./node_modules/randomstring');

module.exports = function (message, ws) {
    ws.hasHandshook = true;
    if (message.type === "game") {
        ws.clientType = "game";
        //assign code
        while (true) {
            let code = randomstring.generate({
                'length': 4,
                'readable': true,
                'charset': 'alphanumeric',
                'capitalization': 'uppercase'
            });
            if (!s.connections.has(code)) {
                ws.send("{\"code\":\""+code+"\"}");
                ws.gameCode = code;
                s.connections.set(code,new s.Session(ws));
                break;
            }
        }
    } else if (message.type === "client") {
        //sanity checking
        if (typeof message.code === 'undefined' || !s.connections.has(message.code)) { //ensure code was provided and has a room
            ws.send("{\"e\":\"Invalid game code.\"}");
            console.log("-Ended connection with "+ws.upgradeReq.socket.remoteAddress+ " (4001)");
            ws.close(4001);
            return;
        }
        if (typeof message.name === 'undefined') {
            ws.send("{\"e\":\"Invalid or nonexistant name.\"}");
            console.log("-Ended connection with "+ws.upgradeReq.socket.remoteAddress+ " (4003)");
            ws.close(4003);
            return;
        }
        //join session
        ws.clientType = "client";
        ws.gameCode = message.code;
        ws.playerName = message.name;
        if (typeof message.identifier === 'undefined') { //new connection
            generateClientIdentifier(ws);
        } else { //TODO possible returning client!
            ws.clientIdentifier = message.identifier;
            if (s.connections.get(ws.gameCode).clients.get(ws.clientIdentifier) === null) { //if we actually have a returning client, reattach this socket to the session
                s.connections.get(ws.gameCode).clients.set(ws.clientIdentifier,ws);
            } else { //if the client is providing what is likely a identifier for a previous game, follow the steps for a new client
                generateClientIdentifier(ws);
            }
        }
        ws.send("{\"joingame\":\"true\"}");
    } else {
        ws.send("{\"e\":\"Invalid type provided on handshake message.\"}");
        console.log("-Ended connection with "+ws.upgradeReq.socket.remoteAddress+" (4000)");
        ws.close(4000);
    }
};

function generateClientIdentifier (ws) {
    do {
        ws.clientIdentifier = randomstring.generate({ //make a random code for referring to this client as
            'length': 32,
            'charset': 'alphanumeric'
        });
    } while (s.connections.get(ws.gameCode).clients.has(ws.clientIdentifier));
    s.connections.get(ws.gameCode).clients.set(ws.clientIdentifier,ws);
    s.connections.get(message.code).server.send("{\"identifier\":\""+ws.clientIdentifier+"\",\"name\":\""+ws.playerName+"\"}"); //notify game of new client
}