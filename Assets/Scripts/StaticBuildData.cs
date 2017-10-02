using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticBuildData {
    public static string gameBuild = "{BUILD_COMMIT}";
    public static bool debug_DetailedTriviaLoading = false;
    public const string wsServAddress = 
#if UNITY_EDITOR
    "ws://144.217.245.39:36245";
#else
    "ws://test.brawlochums.octolopagon.games:36245";
#endif
}
