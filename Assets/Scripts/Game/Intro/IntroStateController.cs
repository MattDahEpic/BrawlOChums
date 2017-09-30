using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class IntroStateController : IGameStateManager {
    public VideoPlayer player;
    public Slider parseProgress;
    public GameObject skipPrompt;

    internal override void SetupHandlers () {} //no special handlers for intro state

	void Start () {
	    sceneLoad = SceneManager.LoadSceneAsync("3trivia");
	    sceneLoad.allowSceneActivation = false;
        //player is set you play on awake
	    parseProgress.maxValue = 1f;
	    parseProgress.minValue = 0f;
	    parseProgress.value = 0;
        skipPrompt.SetActive(false);
	}
	
	void Update () {
	    parseProgress.value = TriviaJSONParser.loadProgress;
	    if (TriviaJSONParser.finishedLoading) {
            skipPrompt.SetActive(true); //show skip prompt
	        if ((ulong) player.frame == player.frameCount || Input.GetKey(KeyCode.Space)) { //skipped or done playing
	            sceneLoad.allowSceneActivation = true;
	        }
	    }
	    //TODO place player names on screen at correct time
        //TODO show max score
	}
}
