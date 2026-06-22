using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

using CarRace.Common;
using CarRace.Infrastructure.SceneLoading;

namespace CarRace.Infrastructure.Services
{
    public class SceneLoadingService : IDisposable
    {
        private SceneId _currentScene = SceneId.Bootstrap;

        private CancellationTokenSource _cts = new CancellationTokenSource();

        public async UniTask LoadSceneAsync(SceneId nextScene, LoadSceneMode mode = LoadSceneMode.Additive)
        {
            if (_currentScene == nextScene)
            {
                GameDebug.LogWarning("SceneLoading", "Current scene index and next scene index is equals");
                return;
            }

            var currentSceneName = Scenes.GetName(_currentScene);
            var nextSceneName = Scenes.GetName(nextScene);

            if (_currentScene != SceneId.Bootstrap)
            {
                await SceneManager.UnloadSceneAsync(currentSceneName).WithCancellation(_cts.Token);
            }

            await SceneManager.LoadSceneAsync(nextSceneName, mode).WithCancellation(_cts.Token);

            _currentScene = nextScene;
        }
        
        public void Dispose()
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }
    }
}