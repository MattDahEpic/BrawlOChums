using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using QRCoder;
using UnityEngine;
using UnityEngine.SceneManagement;
using WebSocketSharp;
using UnityEngine.UI;

public class LobbyManagement : MonoBehaviour {
    public GameObject connectScreen;
    public Text connectScreenMessage;
    public GameObject lobbyScreen;
    public Text gameCode;
    public RawImage qrCode;
    public Text playerNames;
    [HideInInspector] public string playerNamesText = "";
    public Button startGameButton;
    public Slider loadProgress;

    private AsyncOperation gameLoad;

	void /*IEnumerator*/ Start () {
	    connectScreen.SetActive(true);
	    lobbyScreen.SetActive(false);
        playerNames.text = "";
        GameManager.players = new Dictionary<string, GameManager.PlayerStats>();
        //ensure internet is reachable
        /* TODO WWW connectivityTest = new WWW("https://google.com");
	    yield return connectivityTest;
	    if (connectivityTest.error != null) SetConnectionFail();*/
        //start connection
        GameManager.ws = new WebSocket("ws://localhost:36245");
        Debug.Log("Opening connection...");

        GameManager.ws.OnOpen += (sender, e) => {
            GameManager.ws.Send("{\"type\":\"game\"}");
        };
        GameManager.ws.OnMessage += (sender, e) => {
            if (GameManager.gameCode == null) { //has no room code
                Message_RecieveRoomCode code = JsonConvert.DeserializeObject<Message_RecieveRoomCode>(e.Data);
                GameManager.gameCode = code.code;
                Debug.Log("Got code!: "+GameManager.gameCode);
                return;
            }
            try {
                //player join message
                Message_PlayerJoin join = JsonConvert.DeserializeObject<Message_PlayerJoin>(e.Data);
                if (join != null) {
                    GameManager.players.Add(join.identifier,new GameManager.PlayerStats(join.name));
                    playerNamesText = join.name+"\n"+playerNamesText;
                }
            } catch (System.Exception ex) {
                throw ex;
            }
        };
	    GameManager.ws.OnClose += (sender, e) => {
            Debug.Log("Disconnected!");
	        if (GameManager.gameCode == null) {
	            SetConnectionFail();
	        } else {
	            DisconnectCanvas.show = true;
            }
	    };
	    GameManager.ws.Connect();
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
	    playerNames.text = playerNamesText;
	    loadProgress.value = gameLoad.progress;
	    if (gameLoad.progress <= 0.9f) {
	        startGameButton.enabled = false;
	        loadProgress.gameObject.SetActive(true);
	    } else {
	        startGameButton.enabled = true;
            loadProgress.gameObject.SetActive(false);
	    }
	}

    private void OnApplicationQuit() { //TODO ensure that at any point the game crashes the clients are disconnected
        GameManager.ws.Close();
    }

    private void SetConnectionFail () {
        connectScreenMessage.text = "Failed to connect. Check your internet connection and try again!";
    }

    public void StartGame () {
        gameLoad.allowSceneActivation = true;
        //TODO change websocket event handlers
    }
}
