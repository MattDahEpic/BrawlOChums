const HashMap = require('hashmap');

var connections = new HashMap();
module.exports.connections = connections;

class Session {
    constructor(server) {
        this.server = server;
        this.clients = new HashMap();
    }
}
module.exports.Session = Session;