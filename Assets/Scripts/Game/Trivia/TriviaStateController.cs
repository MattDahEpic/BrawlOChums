using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriviaStateController : IGameStateManager {
    internal override void SetupHandlers () {
        //TODO handle receiving of player guesses
    }

    void Start () {
        sceneLoad = SceneManager.LoadSceneAsync("4precomscoreboard");
        //TODO select trivia questions to play and send them to devices
    }
	
	void Update () {
		//TODO send triggers to players when the question starts to display them, if all players answered advance automatically
	}
}
