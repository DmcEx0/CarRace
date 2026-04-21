using CarRace.Factory;
using UnityEngine;

namespace CarRace
{
    public class EnemyFactory : GameObjectFactory
    {
        private readonly EnemiesConfig _config;

        public EnemyFactory(EnemiesConfig config)
        {
            _config = config;
        }
        
        public EnemyContext Get(Vector3 position)
        {
            var instance = Object.Instantiate(_config.Prefab, position, Quaternion.identity);

            var context = new EnemyContext(instance, _config);
            
            return context;
        }
    }
}
