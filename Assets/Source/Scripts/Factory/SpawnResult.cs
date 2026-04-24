using UnityEngine.ResourceManagement.AsyncOperations;

namespace CarRace.Factory
{
    public struct SpawnResult<T>
    {
        public readonly T Instance;
        public readonly AsyncOperationHandle Handle;

        public SpawnResult(T instance, AsyncOperationHandle handle)
        {
            Instance = instance;
            Handle = handle;
        }
    }
}
