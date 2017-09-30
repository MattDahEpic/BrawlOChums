using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;

public static class TriviaJSONParser {
    public static float loadProgress; //TODO if possible file progress (streaming makes it seem iffy)

    public static bool finishedLoading;

    public struct TriviaQuestion {
        [JsonRequired] public string question;
        [JsonRequired] public List<string> answers;
        [JsonRequired] public string correct_answer;
        public string author; //if author is null, dont display an author tag

        public override string ToString () {
            return "[question=" + question + ",answers=" + answers + ",correct_answer=" + correct_answer + ",author=" + author + "]";
        }
    }

    public struct TriviaCategory {
        [JsonRequired] public string name;
        public string description;
        [JsonRequired] public string file;
    }

    private static void LoadFile (string file) {
        JsonSerializer serializer = new JsonSerializer();
        using (JsonReader reader = new JsonTextReader(File.OpenText(file))) { //https://stackoverflow.com/questions/43747477/how-to-parse-huge-json-file-as-stream-in-json-net
            while (reader.Read()) {
                if (reader.TokenType == JsonToken.StartObject) {
                    try {
                        TriviaQuestion q = serializer.Deserialize<TriviaQuestion>(reader);
                        //TODO make sure that all question parts exist
                        if (!q.answers.Contains(q.correct_answer)) {
                            Debug.LogError("Error parsing trivia question "+q.question+" in "+file+"! Answers list did not contain specified correct answer!");
                            continue;
                        }
                        if (StaticBuildData.debug_DetailedTriviaLoading) Debug.Log("Loaded trivia question " + q);
                        GameManager.trivia.Add(q);
                    } catch (JsonException ex) {
                        Debug.LogError("Error parsing trivia question in "+file+"! Error: "+ex.Message);
                    }
                }
            }
        }
    }

    public static void LoadFiles (string[] files) {
        finishedLoading = false;
        int totalFiles = files.Length;
        int currentFile = 1;
        foreach (string file in files) {
            loadProgress = (float)currentFile / totalFiles;
            LoadFile(file);
        }
        finishedLoading = true;
    }

    public static void LoadAllFiles () {
        List<TriviaCategory> lst = JsonConvert.DeserializeObject<List<TriviaCategory>>(File.ReadAllText(Application.streamingAssetsPath+Path.DirectorySeparatorChar+"_triviaindex.json")); //TODO fail fast if file errors
        string[] files = (from cat in lst select cat.file).ToArray(); //TODO prepend Application.streamingAssetsPath, currently puts just a file name
        LoadFiles(files);
    }
}
