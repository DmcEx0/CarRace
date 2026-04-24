using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace CarRace.Factory
{
    public abstract class GameObjectFactory
    {
        protected T Create<T>(T prefab) where T : Object
        {
            var instance = Object.Instantiate(prefab);
            return instance;
        }
        
        protected async UniTask<SpawnResult<T>> CreateWithAddressAsync<T>(AssetReference reference, CancellationToken token)
            where T : Object
        {
            var loadOp = reference.InstantiateAsync();
            var obj = await loadOp.WithCancellation(token);

            if (obj.TryGetComponent(out T instance))
            {
                return new SpawnResult<T>(instance, loadOp);
            }

            throw new Exception($"Can't get component {typeof(T)} from {obj}");
        }
        
        public static void Release(AsyncOperationHandle reference)
        {
            reference.Release();
        }
    }
}