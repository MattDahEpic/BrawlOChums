using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public static class GameManager {
    public static WebSocket ws;
    public static string gameCode = null;
    public static Dictionary<string, PlayerStats> players;

    public class PlayerStats {
        public string name;
        public int score;

        public PlayerStats (string name) {
            this.name = name;
        }
    }
}
