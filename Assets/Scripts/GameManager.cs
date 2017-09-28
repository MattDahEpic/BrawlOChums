using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public static class GameManager {
    public static GameStateHelper.GameState state = GameStateHelper.GameState.LOBBY; //TODO tell the websocketmanager the state changed so it can update handlers
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
