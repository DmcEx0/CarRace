using System.Threading;
using CarRace.Utils;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace CarRace.Services
{
    public class GameInitializeService : IInitializable
    {
        private readonly SceneLoadingService _sceneLoadingService;

        private readonly CancellationTokenSource _cts; //TODO: управлять 

        public GameInitializeService(SceneLoadingService sceneLoadingService)
        {
            _sceneLoadingService = sceneLoadingService;
            _cts = new CancellationTokenSource();
        }
        
        public void Initialize()
        {
            _sceneLoadingService.LoadSceneAsync(Constants.Scenes.BootstrapIndex, Constants.Scenes.HubIndex, _cts.Token).Forget(); 
        }
    }
}