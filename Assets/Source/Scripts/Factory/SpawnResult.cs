using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CarRace.Factory
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
                _handle.Release();
                _handle = default;
            }
        }
    }
}
