using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace CarRace
{
    public static class GameDebug
    {
        [Conditional("GAME_DEBUG")]
        public static void Log(object message) => Debug.Log(message);

        [Conditional("GAME_DEBUG")]
        public static void LogWarning(object message) => Debug.LogWarning(message);

        [Conditional("GAME_DEBUG")]
        public static void LogError(object message) => Debug.LogError(message);
    }
}
