using WebSocketSharp;
using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

public static class WebSocketManager {
    private static WebSocket _ws;

    private static EventHandler<MessageEventArgs> getGameCodeHandler;
    private static EventHandler<MessageEventArgs> playerJoinHandler;

    static WebSocketManager () {
        getGameCodeHandler = (sender, e) => {
            if (GameManager.gameCode == null) { //has no room code
                Message_RecieveRoomCode code = JsonConvert.DeserializeObject<Message_RecieveRoomCode>(e.Data);
                GameManager.gameCode = code.code;
                UnityEngine.Debug.Log("Got code!: " + GameManager.gameCode);
            }
            _ws.OnMessage -= getGameCodeHandler;
        };
        playerJoinHandler = (sender, e) => {
            try {
                //player join message
                Message_PlayerJoin join = JsonConvert.DeserializeObject<Message_PlayerJoin>(e.Data);
                if (join != null) GameManager.players.Add(join.identifier, new GameManager.PlayerStats(join.name));
            } catch {}
        };
    }

     

    public static WebSocket ws {
        get { return _ws; }
    }

    private static bool notInitialized {
        get { return _ws == null; }
    }

    public static void Startup () {
        if (notInitialized) {
            //TODO connectivity test
            UnityEngine.Debug.Log("Opening connection...");
            _ws = new WebSocket("ws://localhost:36245");
            _ws.OnOpen += (sender, e) => {
                _ws.Send("{\"type\":\"game\"}");
            };
            _ws.OnMessage += getGameCodeHandler;
            _ws.OnMessage += playerJoinHandler;

            _ws.Connect();
        }
    }

    public static void Shutdown () {
        if (!notInitialized) {
            //close connection
            _ws.Close();
            _ws = null;
        }
    }
}
