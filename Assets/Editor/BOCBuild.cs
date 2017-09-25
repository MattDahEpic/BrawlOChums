using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BOCBuild {
    public static void BuildWindows () {
		System.Console.WriteLine("Beginning Windows Build");
        string path = System.Environment.GetEnvironmentVariable("WORKSPACE");
        if (path == null) EditorApplication.Exit(1);
        BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, path + @"\Build\Windows.exe", BuildTarget.StandaloneWindows64, BuildOptions.None);
		EditorApplication.Exit(0);
    }
}
