using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BOCMenuItems {
    [MenuItem("Tools/Octolopagon/Build (Test)")]
    private static void TestBuild () {
        System.Environment.SetEnvironmentVariable("WORKSPACE", System.IO.Directory.GetCurrentDirectory());
        Debug.Log("Building player with WORKSPACE="+System.Environment.GetEnvironmentVariable("WORKSPACE"));
        BOCBuild.BuildWindows();
    }
}
