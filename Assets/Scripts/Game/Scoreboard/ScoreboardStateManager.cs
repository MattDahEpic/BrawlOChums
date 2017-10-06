using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ScoreboardStateManager : IGameStateManager {
    public Text scoreboard;

    internal override void SetupHandlers () {}

    private float timer = 10f;

	void Start () {
	    scoreboard.text = "Place".PadRight(7)+"Name".PadRight(25)+"Score".PadRight(9)+"\n";
	    int place = 1;
	    foreach (var kvp in GameManager.players.OrderByDescending(pair => pair.Value.score)) {
	        scoreboard.text += place.ToString().PadRight(7) + kvp.Value.name.PadRight(25) + kvp.Value.score.ToString().PadRight(9) + "\n";
            WebSocketManager.ws.Send("{\"scoreboard\":\""+kvp.Key+"\",\"score\":\""+kvp.Value.score+"\",\"place\":\""+place+"\"}");
	        place++;
	    }
        //TODO decide a set of commercials to play and load the first one
	}

	void Update () {
	    timer -= Time.deltaTime;
	    if (timer <= 0) {
	        //TODO engage loading of first commercial
            Application.Quit();
	    }
	}
}
