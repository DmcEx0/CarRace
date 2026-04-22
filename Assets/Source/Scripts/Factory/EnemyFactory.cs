using CarRace.Factory;
using CarRace.Helpers;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CarRace
{
    public class EnemyFactory : GameObjectFactory
    {
        private readonly EnemiesConfig _config;
        private readonly ObjectPool<EnemyView> _pool;

        public EnemyFactory(EnemiesConfig config, Transform container)
        {
            _config = config;
            _pool = new ObjectPool<EnemyView>(container);
        }
        
        public async UniTask PrepareAsync(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var instance = await CreateWithAddressAsync<EnemyView>(_config.Reference);
                
                _pool.Create(instance.Key);
            }
        }
        
        public EnemyContext Get(Vector3 position)
        {
            var instance = _pool.Get();
            instance.transform.position = position;
            
            var context = new EnemyContext(instance, _config);
            
            return context;
        }
    }
}
