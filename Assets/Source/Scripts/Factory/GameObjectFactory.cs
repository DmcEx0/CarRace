using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace CarRace.Factory
{
    public abstract class GameObjectFactory : IDisposable
    {
        private readonly CancellationTokenSource _tokenSource = new CancellationTokenSource();
        
        protected T Create<T>(T prefab) where T : Object
        {
            var instance = Object.Instantiate(prefab);
            return instance;
        }
        
        protected async UniTask<KeyValuePair<T, AsyncOperationHandle>> CreateWithAddressAsync<T>(AssetReference reference)
            where T : Object
        {
            var loadOp = reference.InstantiateAsync();
            var obj = await loadOp.WithCancellation(_tokenSource.Token);

            if (obj.TryGetComponent(out T instance))
            {
                return new KeyValuePair<T, AsyncOperationHandle>(instance, loadOp);
            }

            throw new Exception($"Can't get component {typeof(T)} from {obj}");
        }
        
        public void Release(AsyncOperationHandle reference)
        {
            reference.Release();
        }

        public void Dispose()
        {
            _tokenSource.Cancel();
            _tokenSource?.Dispose();
        }
    }
}