using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManagement : MonoBehaviour {
    public Text versionNumber;
    private void Update() {
        versionNumber.text = "  " + StaticBuildData.gameBuild;
    }

    public void DoPlay() {
        SceneManager.LoadScene("game_lobby");
    }

    public void DoOptions() {
        throw new NotImplementedException(); //TODO
    }

    public void DoCredits () {
        throw new NotImplementedException(); //TODO
    }

    public void DoQuit() {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
