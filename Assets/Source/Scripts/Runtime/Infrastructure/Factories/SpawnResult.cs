using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CarRace.Infrastructure.Factories
{
    public class SpawnResult<T>
    {
        private  AsyncOperationHandle<GameObject> _handle;
        
        public readonly T Prefab;

        public SpawnResult(T prefab, AsyncOperationHandle<GameObject> handle)
        {
            Prefab = prefab;
            _handle = handle;
        }

        public void Release()
        {
            if(_handle.IsValid())
            {
                Addressables.Release(_handle);
                _handle = default;
            }
        }
    }
}
