using CarRace.Services;
using CarRace.Utils;
using CarRace.Views;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace CarRace.Placeholders
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