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
            GameManager.ws.Close();
            GameManager.gameCode = null;
        } catch {}
    }
}
