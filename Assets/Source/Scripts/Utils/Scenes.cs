using System;

namespace CarRace.Utils
{
    public static class Scenes
    {
        public static string GetName(SceneId scene) => scene switch
        {
            SceneId.Bootstrap => "Bootstrap",
            SceneId.Hub => "Hub",
            SceneId.Level1 => "Level_1",
            _ => throw new ArgumentOutOfRangeException(nameof(scene))
        };
    }
}
