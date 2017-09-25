using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BOCBuild {
    static void BuildWindows () {
        string path = System.Environment.GetEnvironmentVariable("WORKSPACE");
        BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, path + @"\Build\Windows", BuildTarget.StandaloneWindows64, BuildOptions.None);
    }
}
