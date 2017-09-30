using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class IntroStateController : IGameStateManager {
    public VideoPlayer player;

    internal override void SetupHandlers () {} //no special handlers for intro state

	void Start () {
	    sceneLoad = SceneManager.LoadSceneAsync("3trivia");
	    sceneLoad.allowSceneActivation = false;
        //player is set you play on awake
	}
	
	void Update () {
	    if ((ulong)player.frame == player.frameCount) {
	        sceneLoad.allowSceneActivation = true;
	    }
        //TODO place player names on screen at correct time
        //TODO show max score
        //TODO progress bar of trivia loading, don't allow player to skip or scene to advance until the trivia is loaded
	}
}
