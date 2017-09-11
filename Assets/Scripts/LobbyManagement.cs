using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class LobbyManagement : MonoBehaviour {
    public GameObject loadScreen;
    public GameObject lobbyScreen;
	void Start () {
        GameManager.ws = new WebSocket("ws://localhost:36245");
        Debug.Log("Opening connection...");
        GameManager.ws.OnOpen += (sender, e) => {
            GameManager.ws.Send("{\"type\":\"game\"}");
        };
        GameManager.ws.OnMessage += (sender, e) => {
            if (GameManager.gameCode == null) { //has no room code
                //TODO make a Message_RecieveRoomCode out of `e`, set GameManager.gameCode to the code supplied
            }
        };
	}
	
	void Update () {
        if (GameManager.gameCode == null) return; //show loading screen until we have a room code
        loadScreen.SetActive(false);
        lobbyScreen.SetActive(true);
	}
}
