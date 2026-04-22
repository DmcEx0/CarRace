using CarRace.Factory;
using CarRace.Helpers;
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
        
        public void Prepare(int count)
        {
            var instance = Create(_config.Prefab);
            
            _pool.Create(_config.Prefab, count);
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
