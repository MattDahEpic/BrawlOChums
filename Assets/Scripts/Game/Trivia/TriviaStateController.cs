using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriviaStateController : IGameStateManager {
    private List<TriviaJSONParser.TriviaQuestion> selectedQuestions = new List<TriviaJSONParser.TriviaQuestion>();
    internal override void SetupHandlers () {
        //TODO handle receiving of player guesses
    }

    void Start () {
        sceneLoad = SceneManager.LoadSceneAsync("4precomscoreboard");
        //decide questions
        for (int i = 0; i < 5; i++) {
            selectedQuestions.Add(GameManager.trivia[Random.Range(0,GameManager.trivia.Count)]);
        }
        //send them to devices
        WebSocketManager.ws.Send("{" +
                                    "\"trivia\":\"loadquestions\"," +
                                    "\"questions\":{" +
                                        "\"question1\":{" +
                                            "\"question\":\""+selectedQuestions[0].question+"\"," +
                                            "\"answers\":[" +
                                                "\""+selectedQuestions[0].answers[0]+"\"," +
                                                "\""+selectedQuestions[0].answers[1]+"\"," +
                                                "\""+selectedQuestions[0].answers[2]+"\"," +
                                                "\""+selectedQuestions[0].answers[3]+"\"" +
                                            "]" +
                                        "}," +
                                        "\"question2\":{" +
                                            "\"question\":\""+selectedQuestions[1].question+"\"," +
                                            "\"answers\":[" +
                                                "\""+selectedQuestions[1].answers[0]+"\"," +
                                                "\""+selectedQuestions[1].answers[1]+"\"," +
                                                "\""+selectedQuestions[1].answers[2]+"\"," +
                                                "\""+selectedQuestions[1].answers[3]+"\"" +
                                            "]" +
                                        "}," +
                                        "\"question3\":{" +
                                            "\"question\":\""+selectedQuestions[2].question+"\"," +
                                            "\"answers\":[" +
                                                "\""+selectedQuestions[2].answers[0]+"\"," +
                                                "\""+selectedQuestions[2].answers[1]+"\"," +
                                                "\""+selectedQuestions[2].answers[2]+"\"," +
                                                "\""+selectedQuestions[2].answers[3]+"\"" +
                                            "]" +
                                        "}," +
                                        "\"question4\": {" +
                                            "\"question\":\""+selectedQuestions[3].question+"\"," +
                                            "\"answers\":[" +
                                                "\""+selectedQuestions[3].answers[0]+"\"," +
                                                "\""+selectedQuestions[3].answers[1]+"\"," +
                                                "\""+selectedQuestions[3].answers[2]+"\"," +
                                                "\""+selectedQuestions[3].answers[3]+"\"" +
                                            "]" +
                                        "}," +
                                        "\"question5\":{" +
                                            "\"question\":\""+selectedQuestions[4].question+"\"," +
                                            "\"answers\":[" +
                                                "\""+selectedQuestions[4].answers[0]+"\"," +
                                                "\""+selectedQuestions[4].answers[1]+"\"," +
                                                "\""+selectedQuestions[4].answers[2]+"\"," +
                                                "\""+selectedQuestions[4].answers[3]+"\"" +
                                            "]" +
                                        "}" +
                                    "}" +
                                 "}");
    }
	
	void Update () {
		//TODO send triggers to players when the question starts to display them, if all players answered advance automatically
	}
}
