/*
Error Codes:
4000: type not provided on handshake (didn't identify as game or client)
4001: client provided a game code that didn't exist
4002: game ended because room destructed (game client left)
4003: client attempted to connect without a name or the name provided was invalid
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
    console.log("Received SIGINT, stopping gracefully...");
    wss.clients.forEach(function (ws) {
        console.log("-Ended connection with "+ws.upgradeReq.socket.remoteAddress+" (1001)");
        ws.close(1001);
    });
    process.exit(1);
});

wss.on('connection',function connection(ws,conn) {
   console.log("+Received connection from "+ws._socket.remoteAddress);
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
                   console.log("-Ended connection with "+ws._socket.remoteAddress+ " (4001)");
                   ws.close(4001);
                   return;
               }
               if (typeof message.name === 'undefined') {
                   ws.send("{\"e\":\"Invalid or nonexistant name.\"}");
                   console.log("-Ended connection with "+ws._socket.remoteAddress+ " (4003)");
                   ws.close(4003);
                   return;
               }
               //attach client to game session
               ws.clientType = "client";
               ws.gameCode = message.code;
               ws.playerName = message.name;
               connections.get(message.code).clients.push(ws);
               ws.send("{\"joingame\":\"true\"}");
               //TODO send an initial update to force the client to show graphics
           } else {
               ws.send("{\"e\":\"Invalid type provided on handshake message.\"}");
               console.log("-Ended connection with "+ws._socket.remoteAddress+" (4000)");
               ws.close(4000);
           }
       }
   };
   ws.onclose = function close() {
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