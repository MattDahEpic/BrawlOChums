using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class TriviaStateController : IGameStateManager {
    public VideoPlayer introVideo;
    public Text question;
    public Text answer1;
    public Text answer2;
    public Text answer3;
    public Text answer4;
    public Text roundProgress;
    public Slider timeSlider;

    private float timer;
    private int currentQuestion;
    private bool displayingResults;
    private List<TriviaJSONParser.TriviaQuestion> selectedQuestions = new List<TriviaJSONParser.TriviaQuestion>();
    private Dictionary<string, Dictionary<int, bool>> questionAnsweredState = new Dictionary<string, Dictionary<int, bool>>();
    private Dictionary<string, int> currentQuestionVotes = new Dictionary<string, int>();

    internal override void SetupHandlers () {
        onMessage = (sender, msg) => {
            try {
                Message_TriviaResponse resp = JsonConvert.DeserializeObject<Message_TriviaResponse>(msg.Data);
                if (timer > 0) {
                    //ensure player has not answered question already
                    if (questionAnsweredState.ContainsKey(resp.indentifier)) {
                        if (questionAnsweredState[resp.indentifier].ContainsKey(currentQuestion)) {
                            if (questionAnsweredState[resp.indentifier][currentQuestion]) return;
                        }
                    } else {
                        questionAnsweredState[resp.indentifier] = new Dictionary<int, bool>();
                    }
                    //and if they havent, count their vote
                    questionAnsweredState[resp.indentifier][currentQuestion] = true;
                    currentQuestionVotes[resp.selected]++;
                    if (resp.selected == selectedQuestions[currentQuestion].correct_answer) { //award points for correct answer
                        GameManager.players[resp.indentifier].score += Mathf.FloorToInt(1000 * (timer / GameManager.triviaQuestionTime));
                    }
                }
            } catch { }
        };
    }

    void Start () {
        sceneLoad = SceneManager.LoadSceneAsync("4precomscoreboard");
        sceneLoad.allowSceneActivation = false;
        //decide questions
        for (int i = 0; i < 5; i++) {
            selectedQuestions.Add(GameManager.trivia[Random.Range(0,GameManager.trivia.Count)]);
        }
    }
	
	void Update () {
	    timer -= Time.deltaTime;
        roundProgress.text = (currentQuestion+1)+"/5";
	    timeSlider.value = timer / GameManager.triviaQuestionTime;
	    if (introVideo.gameObject.activeInHierarchy) {
	        if ((ulong) introVideo.frame == introVideo.frameCount) {
	            introVideo.gameObject.SetActive(false);
	            SetupQuestion(0);
	        }
	        return;
	    }
	    if (timer <= 0) { //TODO if all connected players have answered the question advance anyways
	        if (displayingResults) {
	            if (currentQuestion + 1 >= 5) { //if final question, advance to scoreboard
	                sceneLoad.allowSceneActivation = true;
	                return;
	            }
	            timeSlider.gameObject.SetActive(true);
                SetupQuestion(currentQuestion+1);
	        } else {
                timeSlider.gameObject.SetActive(false);
	            //display results TODO vote percentages
	            int votes = currentQuestionVotes.Values.Sum();
	            answer1.color = answer1.text == selectedQuestions[currentQuestion].correct_answer ? Color.green : Color.red;
	            //answer1.text += "\n" + (currentQuestionVotes.ContainsKey(answer1.text) ? Mathf.FloorToInt((currentQuestionVotes[answer1.text] / votes)*100) : 0) + "%";
	            answer2.color = answer2.text == selectedQuestions[currentQuestion].correct_answer ? Color.green : Color.red;
	            //answer2.text += "\n" + (currentQuestionVotes.ContainsKey(answer2.text) ? Mathf.FloorToInt((currentQuestionVotes[answer2.text] / votes) * 100) : 0) + "%";
                answer3.color = answer3.text == selectedQuestions[currentQuestion].correct_answer ? Color.green : Color.red;
	            //answer3.text += "\n" + (currentQuestionVotes.ContainsKey(answer3.text) ? Mathf.FloorToInt((currentQuestionVotes[answer3.text] / votes) * 100) : 0) + "%";
                answer4.color = answer4.text == selectedQuestions[currentQuestion].correct_answer ? Color.green : Color.red;
	            //answer4.text += "\n" + (currentQuestionVotes.ContainsKey(answer4.text) ? Mathf.FloorToInt((currentQuestionVotes[answer4.text] / votes) * 100) : 0) + "%";
                WebSocketManager.ws.Send("{\"trivia\":\"hidequestion\"}"); //during results hide the question if the player hasn't answered it
	            displayingResults = true;
	            timer = 3f;
	        }
	    }
	}

    private void SetupQuestion (int questionIndex) {
        TriviaJSONParser.TriviaQuestion q = selectedQuestions[questionIndex];
        currentQuestion = questionIndex;
        currentQuestionVotes.Clear();
        displayingResults = false;
        //push details to screen
        question.text = q.question;
        q.answers.Shuffle();
        answer1.color = Color.white;
        answer1.text = q.answers[0];
        answer2.color = Color.white;
        answer2.text = q.answers[1];
        answer3.color = Color.white;
        answer3.text = q.answers[2];
        answer4.color = Color.white;
        answer4.text = q.answers[3];
        //let client know and start timer
        WebSocketManager.ws.Send("{" +
                                    "\"trivia\":\"displayquestion\"," +
                                    "\"question\":{" +
                                        "\"question\":\"" + selectedQuestions[currentQuestion].question + "\"," +
                                        "\"answers\":[" +
                                            "\"" + selectedQuestions[currentQuestion].answers[0] + "\"," +
                                            "\"" + selectedQuestions[currentQuestion].answers[1] + "\"," +
                                            "\"" + selectedQuestions[currentQuestion].answers[2] + "\"," +
                                            "\"" + selectedQuestions[currentQuestion].answers[3] + "\"" +
                                        "]" +
                                    "}" +
                                 "}");
        timer = GameManager.triviaQuestionTime;
    }
}
