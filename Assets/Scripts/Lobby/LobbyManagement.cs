using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using QRCoder;
using UnityEngine;
using WebSocketSharp;
using UnityEngine.UI;

public class LobbyManagement : MonoBehaviour {
    public GameObject connectScreen;
    public Text connectScreenMessage;
    public GameObject lobbyScreen;
    public Text gameCode;
    public RawImage qrCode;

	void /*IEnumerator*/ Start () {
	    connectScreen.SetActive(true);
	    lobbyScreen.SetActive(false);
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
            //TODO handle player join messages
        };
	    GameManager.ws.Connect();
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
	}

    private void SetConnectionFail () {
        connectScreenMessage.text = "Failed to connect. Check your internet connection and try again!";
    }
}
