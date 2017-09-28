using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameStateHelper {
    public enum GameState {
        LOBBY,
        INTRO,
        TRIVIA_NARRATION,
        TRIVIA_ANSWER_COLLECTION,
        TRIVIA_ANSWER_TALLY,
        SCOREBOARD_PRECOMMERCIAL,
        COMMERCIAL_INTRO,
        COMMERCIAL_GAMEPLAY,
        COMMERCIAL_ANSWER_TALLY,
        SCOREBOARD_POSTCOMMERCIAL,

    }
}
