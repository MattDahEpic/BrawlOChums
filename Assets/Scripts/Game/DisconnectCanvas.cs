using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisconnectCanvas : MonoBehaviour {
    public static bool show = false;
	
	void Update () {
		gameObject.SetActive(show); //TODO pause game?
	}

    public void ClickButton () {
        try {
            WebSocketManager.Shutdown();
            GameManager.gameCode = null;
        } catch {}
    }
}
