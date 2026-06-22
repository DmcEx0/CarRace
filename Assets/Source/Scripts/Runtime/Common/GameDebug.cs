using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace CarRace.Common
{
    public static class GameDebug
    {
        [Conditional("GAME_DEBUG")]
        public static void Log(object message) => Debug.Log(message);

        [Conditional("GAME_DEBUG")]
        public static void Log(string type, object message) => Debug.Log($"[{type}] {message}");

        [Conditional("GAME_DEBUG")]
        public static void LogWarning(object message) => Debug.LogWarning(message);
        
        [Conditional("GAME_DEBUG")]
        public static void LogWarning(string type,object message) => Debug.LogWarning($"[{type}] {message}");

        [Conditional("GAME_DEBUG")]
        public static void LogError(object message) => Debug.LogError(message);
        
        [Conditional("GAME_DEBUG")]
        public static void LogError(string type, object message) => Debug.LogError($"[{type}] {message}");
    }
}
