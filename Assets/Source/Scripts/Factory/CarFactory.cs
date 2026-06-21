using System.Threading;
using CarRace.Views;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CarRace.Factory
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