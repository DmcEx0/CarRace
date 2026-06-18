using System.Threading;
using CarRace.Utils;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace CarRace.Services
{
    public class GameInitializeService : IInitializable
    {
        private readonly SceneLoadingService _sceneLoadingService;

        public GameInitializeService(SceneLoadingService sceneLoadingService)
        {
            _sceneLoadingService = sceneLoadingService;
        }
        
        public void Initialize()
        {
            _sceneLoadingService.LoadSceneAsync(Constants.Scenes.BootstrapIndex, Constants.Scenes.HubIndex, new CancellationToken()).Forget(); //TODO: заменить на конкретный
        }
    }
}