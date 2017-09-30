using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BOCMenus {
    [MenuItem("Tools/Octolopagon/Attempt Loading All Trivia")]
    private static void AttemptLoadAllTrivia () {
        new System.Threading.Thread(() => { //start the processing in a thread so the intro can run while it sifts through (likely) gigabytes of trivia questions
            TriviaJSONParser.LoadAllFiles();
        }).Start();
    }
}
