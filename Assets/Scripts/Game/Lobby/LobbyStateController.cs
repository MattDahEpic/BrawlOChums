using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    public Transform categorySelectContent;
    public GameObject categorySelectPrefab;

    private List<CategorySelectRenderer> categorySelectors = new List<CategorySelectRenderer>();

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
        //populate category list
        List<TriviaJSONParser.TriviaCategory> lst = JsonConvert.DeserializeObject<List<TriviaJSONParser.TriviaCategory>>(File.ReadAllText(Application.streamingAssetsPath + Path.DirectorySeparatorChar + "_triviaindex.json")); //TODO fail fast if file errors
        int ySpawn = -25;
        foreach (TriviaJSONParser.TriviaCategory cat in lst) {
            GameObject g = Instantiate(categorySelectPrefab, categorySelectContent);
            g.transform.localPosition = new Vector3(0,ySpawn,0);
            CategorySelectRenderer catsel = g.GetComponent<CategorySelectRenderer>();
            catsel.category = cat;
            categorySelectors.Add(catsel);
            ySpawn -= 50;
        }
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
	}

    private void SetConnectionFail () {
        connectScreenMessage.text = "Failed to connect. Check your internet connection and try again!";
    }

    public void StartGame () {
        //begin loading of trivia
        List<string> files = new List<string>();
        foreach (CategorySelectRenderer catsel in categorySelectors) {
            files.Add(Application.streamingAssetsPath+Path.DirectorySeparatorChar+catsel.category.file);
        }
        //TODO handle case if there's no trivia selected
        new System.Threading.Thread(() => { //start the processing in a thread so the intro can run while it sifts through (likely) gigabytes of trivia questions
            TriviaJSONParser.LoadFiles(files.ToArray());
        }).Start();
        //load intro scene
        sceneLoad.allowSceneActivation = true;
    }
}
