using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BOCBuild {
    public static void BuildWindows () {
		System.Console.WriteLine("Beginning Windows Build");
        string path = System.Environment.GetEnvironmentVariable("WORKSPACE");
        if (path == null) EditorApplication.Exit(1);
        BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, path + @"\Build\BOC-Windows.exe", BuildTarget.StandaloneWindows64, BuildOptions.None);
		EditorApplication.Exit(0);
    }
    public static void BuildMac () {
        System.Console.WriteLine("Beginning Mac Build");
        string path = System.Environment.GetEnvironmentVariable("WORKSPACE");
        if (path == null) EditorApplication.Exit(1);
        BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, path + @"\Build\BOC-Mac.app", BuildTarget.StandaloneOSXUniversal, BuildOptions.None);
        EditorApplication.Exit(0);
    }
    public static void BuildLinux () {
        System.Console.WriteLine("Beginning Linux Build");
        string path = System.Environment.GetEnvironmentVariable("WORKSPACE");
        if (path == null) EditorApplication.Exit(1);
        BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, path + @"\Build\BOC-Linux.x86", BuildTarget.StandaloneLinuxUniversal, BuildOptions.None);
        EditorApplication.Exit(0);
    }
}
