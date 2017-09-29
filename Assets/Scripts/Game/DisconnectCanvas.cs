using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DisconnectCanvas : MonoBehaviour {
    public static bool show = false;
	
	void Update () {
		gameObject.SetActive(show); //TODO pause game?
	}

    public void ClickButton () {
        try {
            WebSocketManager.Shutdown();
            SceneManager.LoadScene("0menu");
        } catch {}
    }
}
