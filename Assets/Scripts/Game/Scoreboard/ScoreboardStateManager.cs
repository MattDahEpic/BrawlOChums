using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ScoreboardStateManager : IGameStateManager {
    public Text scoreboard;
    internal override void SetupHandlers () {}

	void Start () {
	    scoreboard.text = "Place".PadRight(7)+"Name".PadRight(25)+"Score".PadRight(9)+"\n";
	    int place = 1;
	    foreach (var kvp in GameManager.players.OrderByDescending(pair => pair.Value.score).Take(25)) {
	        scoreboard.text += place.ToString().PadRight(7) + kvp.Value.name.PadRight(25) + kvp.Value.score.ToString().PadRight(9) + "\n";
            //TODO send scores to devices
	    }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
