using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using QRCoder;
using UnityEngine;
using UnityEngine.SceneManagement;
using WebSocketSharp;
using UnityEngine.UI;

public class LobbyManagement : IGameStateManager {
    public GameObject connectScreen;
    public Text connectScreenMessage;
    public GameObject lobbyScreen;
    public Text gameCode;
    public RawImage qrCode;
    public Text playerNames;
    public Button startGameButton;
    public Slider loadProgress;

    private AsyncOperation gameLoad;

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
        //ensure internet is reachable
        /* TODO WWW connectivityTest = new WWW("https://google.com");
	    yield return connectivityTest;
	    if (connectivityTest.error != null) SetConnectionFail();*/
        //connect websocket
        WebSocketManager.Startup();
        //load first scene async
	    gameLoad = SceneManager.LoadSceneAsync("game");
	    gameLoad.allowSceneActivation = false;
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
	    foreach (GameManager.PlayerStats p in GameManager.players.Values) { //populate player name list
	        playerNames.text += p.name + "\n";
	    }
	    loadProgress.value = gameLoad.progress;
	    if (gameLoad.progress <= 0.9f) {
	        startGameButton.enabled = false;
	        loadProgress.gameObject.SetActive(true);
	    } else {
	        startGameButton.enabled = true;
            loadProgress.gameObject.SetActive(false);
	    }
	}

    private void SetConnectionFail () {
        connectScreenMessage.text = "Failed to connect. Check your internet connection and try again!";
    }

    public void StartGame () {
        gameLoad.allowSceneActivation = true;
        //TODO change websocket event handlers
    }
}
