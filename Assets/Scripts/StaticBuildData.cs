using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticBuildData {
    public static string gameBuild = "{BUILD_COMMIT}";
    public static bool debug_DetailedTriviaLoading = false;
    public const string wsServAddress = 
#if UNITY_EDITOR
    "ws://dev.brawlochums.gq/ws/";
#else
    "ws://brawlochums.gq/ws/";
#endif
}
