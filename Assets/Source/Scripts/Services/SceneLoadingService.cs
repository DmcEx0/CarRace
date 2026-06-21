using System.Threading;
using CarRace.Utils;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace CarRace.Services
{
    public class SceneLoadingService
    {
        public async UniTask LoadSceneAsync(SceneId currentScene, SceneId nextScene, CancellationToken token,
            LoadSceneMode mode = LoadSceneMode.Additive)
        {
            if (currentScene == nextScene)
            {
                GameDebug.LogWarning("SceneLoading", "Current scene index and next scene index is equals");
                return;
            }
            
            var currentSceneName = Scenes.GetName(currentScene);
            var nextSceneName = Scenes.GetName(nextScene);

            if (currentScene != SceneId.Bootstrap)
            {
                await SceneManager.UnloadSceneAsync(currentSceneName).WithCancellation(token);
            }

            await SceneManager.LoadSceneAsync(nextSceneName, mode).WithCancellation(token);;
        }
    }
}