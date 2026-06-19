using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace CarRace.Factory
{
    public abstract class GameObjectFactory
    {
        protected T Create<T>(T prefab) where T : Component
        {
            var instance = Object.Instantiate(prefab);
            return instance;
        }
        
        protected T Create<T>(T prefab, Transform parent) where T : Component
        {
            var instance = Object.Instantiate(prefab, parent);
            return instance;
        }

        // protected async UniTask<SpawnResult<T>> CreateWithAddressAsync<T>(AssetReference reference,
        //     CancellationToken token)
        //     where T : Object
        // {
        //     var loadOp = reference.InstantiateAsync();
        //     var obj = await loadOp.WithCancellation(token);
        //
        //     if (obj.TryGetComponent(out T instance) == false)
        //     {
        //         Release1(loadOp);
        //         throw new Exception($"Can't get component {typeof(T)} from {obj}");
        //     }
        //
        //     return new SpawnResult<T>(instance, loadOp);
        // }
        
        protected async UniTask<SpawnResult<T>> CreateWithAddressAsync<T>(AssetReference reference,
            CancellationToken token)
            where T : Object
        {
            var loadOp = reference.LoadAssetAsync<GameObject>();
            var obj = await loadOp.WithCancellation(token);

            if (obj.TryGetComponent(out T component) == false)
            {
                loadOp.Release();
                throw new Exception($"Can't get component {typeof(T)} from {obj}");
            }

            return new SpawnResult<T>(component, loadOp);
        }
    }
}