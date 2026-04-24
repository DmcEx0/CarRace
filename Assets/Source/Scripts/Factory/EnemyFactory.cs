using System.Collections.Generic;
using System.Threading;
using CarRace.Factory;
using CarRace.Helpers;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CarRace
{
    public class EnemyFactory : GameObjectFactory
    {
        private readonly EnemiesConfig _config;
        private readonly ObjectPool<EnemyView> _pool;
        
        private readonly List<AsyncOperationHandle> _operationHandles;

        public EnemyFactory(EnemiesConfig config, Transform container)
        {
            _config = config;
            _pool = new ObjectPool<EnemyView>(container);
            _operationHandles = new List<AsyncOperationHandle>();
        }
        
        public async UniTask PrepareAsync(int count, CancellationToken token)
        {
            for (int i = 0; i < count; i++)
            {
                var result = await CreateWithAddressAsync<EnemyView>(_config.Reference, token);
                
                _operationHandles.Add(result.Handle);
                
                _pool.AddInstance(result.Instance);
            }
        }
        
        public EnemyContext Get(Vector3 position)
        {
            var instance = _pool.Get();
            instance.transform.position = position;
            
            var context = new EnemyContext(instance, _config);
            
            return context;
        }

        public void ReleaseAll()
        {
            foreach (var operationHandle in _operationHandles)
            {
                Release(operationHandle);
            }
            
            _operationHandles.Clear();
        }
    }
}
