using CarRace.Utils;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CarRace.Services
{
    public class SceneLoadingService
    {
        public async UniTask LoadSceneAsync(int nextSceneIndex, int currentSceneIndex, LoadSceneMode mode)
        {
            if (currentSceneIndex == nextSceneIndex)
            {
                GameDebug.LogWarning("SceneLoading", "Current scene index and next scene index is equals");
                return;
            }

            if (currentSceneIndex != Constants.Scenes.BootstrapIndex)
            {
                await SceneManager.UnloadSceneAsync(currentSceneIndex);
            }

            await SceneManager.LoadSceneAsync(nextSceneIndex, mode);
        }
    }
}