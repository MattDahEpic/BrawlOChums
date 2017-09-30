using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using QRCoder;
using UnityEngine;
using UnityEngine.SceneManagement;
using WebSocketSharp;
using UnityEngine.UI;

public class LobbyStateController : IGameStateManager {
    public GameObject connectScreen;
    public Text connectScreenMessage;
    public GameObject lobbyScreen;
    public Text gameCode;
    public RawImage qrCode;
    public Text playerNames;
    public Button startGameButton;
    public Slider loadProgress;

    internal override void SetupHandlers () {
        onClose = (sender, e) => {
            Debug.Log("Disconnected!");
            if (GameManager.gameCode == null) {
                SetConnectionFail();
            } else {
                DisconnectCanvas.show = true;
            }
        };
    }


    void /*IEnumerator*/ Start () {
	    connectScreen.SetActive(true);
	    lobbyScreen.SetActive(false);
        playerNames.text = "";
        GameManager.players = new Dictionary<string, GameManager.PlayerStats>();
        GameManager.trivia = new List<TriviaJSONParser.TriviaQuestion>();
        //connect websocket
        WebSocketManager.Startup();
        //load first scene async
	    sceneLoad = SceneManager.LoadSceneAsync("2intro");
	    sceneLoad.allowSceneActivation = false;
	}
	
	void Update () {
	    if (GameManager.gameCode == null) { //show loading screen until we have a room code
            if (Time.timeSinceLevelLoad > 10f) SetConnectionFail();
	        return;
        }
        connectScreen.SetActive(false);
        lobbyScreen.SetActive(true);
	    gameCode.text = GameManager.gameCode;
	    qrCode.texture = new UnityQRCode(new QRCodeGenerator().CreateQrCode("https://brawlochums.live#" + GameManager.gameCode, QRCodeGenerator.ECCLevel.H)).GetGraphic(60);
	    playerNames.text = "";
	    foreach (GameManager.PlayerStats p in GameManager.players.Values) { //populate player name list
	        playerNames.text += p.name + "\n";
	    }
	    loadProgress.value = sceneLoad.progress;
	    if (sceneLoad.progress < 0.9f) {
	        startGameButton.enabled = false;
	        loadProgress.gameObject.SetActive(true);
	    } else {
	        startGameButton.enabled = true;
            loadProgress.gameObject.SetActive(false);
	    }
	    //TODO show trivia category options, prepare those for loading at game start
	}

    private void SetConnectionFail () {
        connectScreenMessage.text = "Failed to connect. Check your internet connection and try again!";
    }

    public void StartGame () {
        sceneLoad.allowSceneActivation = true;
        //TODO start trivia parsing
    }
}
