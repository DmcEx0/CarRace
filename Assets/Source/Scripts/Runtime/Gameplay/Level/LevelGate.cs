using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

using CarRace.Gameplay.Car;
using CarRace.Infrastructure.SceneLoading;
using CarRace.Infrastructure.Services;

namespace CarRace.Gameplay.Level
{
    public class LevelGate : MonoBehaviour
    {
        [SerializeField] private SceneId _nextLvlId;
        
        private SceneLoadingService _sceneLoadingService;
        
        [Inject]
        private void Construct(SceneLoadingService sceneLoadingService)
        {
            _sceneLoadingService = sceneLoadingService;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent(out CarView carView))
            {
                _sceneLoadingService.LoadSceneAsync(_nextLvlId).Forget();
            }
        }
    }
}