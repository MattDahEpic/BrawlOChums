/*
Error Codes:
4000: type not provided on handshake (didn't identify as game or client)
4001: client provided a game code that didn't exist
4002: game ended because room destructed (game client left)
4003: client attempted to connect without a name or the name provided was invalid
 */

const websock = require('ws');
const fs = require('fs');

const struct = require('./boc_struct.js');
const boc_handshake = require('./boc_handshake.js');
const boc_trivia = require('./boc_trivia.js');

console.log("Starting VoxelatedAvacado server.");
const wss = new websock.Server({ port: 36245, path: "/ws/"});

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
       let message;
       try {
           message = JSON.parse(msg.data);
       } catch (ex) {
           ws.send("{\"e\":\"Invalid json.\"}");
           return;
       }
       if (!ws.hasHandshook) boc_handshake(message,ws);
       boc_trivia(msg.data,message,ws);
   };

   ws.onclose = function close() {
       if (ws.clientType === "game") { //kill room with ws.gameCode, and all clients attached to it
           struct.connections.get(ws.gameCode).clients.forEach(function (client) {
                if (client !== null) client.close(4002);
           });
           struct.connections.delete(ws.gameCode);
       } else { //mark client as gone, but leave the code in the list so if it returns it can take the same spot
           if (typeof ws.gameCode === 'undefined') return; //is disconnecting prior to entering room (invalid code?)
           if (!struct.connections.has(ws.gameCode)) return; //onclose firing because of client disconnection due to server close, room has already died.
           struct.connections.get(ws.gameCode).clients.set(ws.clientIdentifier,null);
       }
   }
});