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
        
        public void Prepare()
        {
            _pool.Create(_config.Prefab, 25);
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
