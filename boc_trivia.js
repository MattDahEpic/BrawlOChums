const s = require('./boc_struct.js');

module.exports = function (messageRaw,message, ws) {
    if (message.trivia === "loadquestions" && ws.clientType === "game") { //recieving questions from game to send to clients
        const clients = s.connections.get(ws.gameCode).clients.values();
        for (let i = 0; i < clients.length; i++) {
            clients[i].send(messageRaw);
        }
    } else if (false) { //TODO game trigger to show question

    } else if (false) { //TODO client sending answers to server

    }
};