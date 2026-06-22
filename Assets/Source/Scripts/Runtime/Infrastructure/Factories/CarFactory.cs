using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

using CarRace.Gameplay.Car;

namespace CarRace.Infrastructure.Factories
{
    public class CarFactory : GameObjectFactory
    {
        public async UniTask<CarView> GetAsync(AssetReference reference, CancellationToken token)
        {
            var spawnResult = await CreateWithAddressAsync<CarView>(reference, token);
            
            var instance = Create(spawnResult.Prefab);
            
            instance.transform.position = Vector3.zero;
            
            return instance;
        }
    }
}