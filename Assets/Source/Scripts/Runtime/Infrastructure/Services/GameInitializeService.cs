using Cysharp.Threading.Tasks;
using VContainer.Unity;

using CarRace.Infrastructure.SceneLoading;

namespace CarRace.Infrastructure.Services
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
            _sceneLoadingService.LoadSceneAsync(SceneId.Hub).Forget(); 
        }
    }
}