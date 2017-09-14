/*
Error Codes:
4000: type not provided on handshake (didn't identify as game or client)
4001: client provided a game code that didn't exist
4002: game ended because room destructed (game client left)
 */

const websock = require('./node_modules/ws');
const HashMap = require('./node_modules/hashmap');
const jsonparse = require('./node_modules/jsonparse');
const randomstring = require('./node_modules/randomstring');

class Session {
    constructor(server) {
        this.server = server;
        this.clients = [];
    }
}

console.log("Starting VoxelatedAvacado server.");

var connections = new HashMap();
const json = new jsonparse();

const wss = new websock.Server({ port: 36245 });

process.on('SIGINT',function () {
    console.log("Recieved SIGINT, stopping gracefully...");
    wss.clients.forEach(function (ws) {
        console.log("-Ended connection with "+ws.upgradeReq.socket.remoteAddress+" (1001)");
        ws.closeReasonCode = 1001;
        ws.close();
    });
    process.exit(1);
});

wss.on('connection',function connection(ws,conn) {
   console.log("+Recieved connection from "+ws._socket.remoteAddress);
   ws.upgradeReq = conn;
   ws.hasHandshook = false;
   ws.onmessage = function message(msg) {
       var message;
       try {
           message = JSON.parse(msg.data);
       } catch (ex) {
           ws.send("{\"e\":\"Invalid json.\"}");
           return;
       }
       if (!ws.hasHandshook) {
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
                   if (!connections.has(code)) {
                       ws.send("{\"code\":\""+code+"\"}");
                       ws.gameCode = code;
                       connections.set(code,new Session(ws));
                       break;
                   }
               }
           } else if (message.type === "client") {
               //ensure code was provided and has a room
               if (typeof message.code === 'undefined' || !connections.has(message.code)) {
                   ws.send("{\"e\":\"Invalid game code.\"}");
                   ws.closeReasonCode = 4001;
                   ws.closeDescription = "Invalid game code.";
                   console.log("-Ended connection with "+ws._socket.remoteAddress+ " (4001)");
                   ws.close();
               }
               if (typeof message.name === 'undefined') {
                   //TODO error out, no player name provided
                   return;
               }
               //attach client to game session
               ws.clientType = "client";
               ws.gameCode = message.code;
               ws.playerName = message.name;
               connections.get(message.code).clients.add(ws);
               ws.send("{\"joingame\":\"true\"}");
               //TODO tell the client it's been added and send an initial update to force the client to show graphics
           } else {
               ws.send("{\"e\":\"Invalid type provided on handshake message.\"}");
               ws.closeReasonCode = 4000;
               ws.closeDescription = "Invalid type provided on handshake message.";
               console.log("-Ended connection with "+ws._socket.remoteAddress+" (4000)");
               ws.close();
           }
       }
   };
   ws.onclose = function close() {
       console.log("-Ended connection with "+ws.upgradeReq.socket.remoteAddress+" (Client Closed)");
       if (ws.clientType == "game") {
           return;
            //TODO kill room with ws.gameCode, and all clients attached to it
       } else { //client
           return;
            //TODO mark as gone OR fully disconnect, depending on how we want to do things
       }
       //TODO if server, kill any rooms attached to it
       //TODO if client mark it as gone
   }
});