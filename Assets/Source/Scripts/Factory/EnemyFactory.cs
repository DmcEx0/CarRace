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
            _pool.Create();
        }
        
        public EnemyContext Get(Vector3 position)
        {
            var instance = Object.Instantiate(_config.Prefab, position, Quaternion.identity);

            var context = new EnemyContext(instance, _config);
            
            return context;
        }
    }
}
