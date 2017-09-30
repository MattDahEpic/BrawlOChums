const s = require('./boc_struct.js');

module.exports = function (messageRaw,message, ws) {
    //recieving questions from game to send to clients
    //OR game trigger to display questions on client
    if ((message.trivia === "loadquestions" || message.trivia === "displayquestion") && ws.clientType === "game") {
        const clients = s.connections.get(ws.gameCode).clients.values();
        for (let i = 0; i < clients.length; i++) {
            clients[i].send(messageRaw);
        }
    } else if (message.trivia === "response" && ws.clientType === "client") { //client sending answers to server
        s.connections.get(ws.gameCode).server.send("{\"trivia\":\"response\",\"identifier\":\""+ws.clientIdentifier+"\",\"selected\":\""+message.selected+"\"}");
    }
};