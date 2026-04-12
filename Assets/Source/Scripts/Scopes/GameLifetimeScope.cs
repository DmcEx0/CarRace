using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace CarRace
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private EnemyView _enemyViewPrefab;
        [SerializeField] private EnemiesConfig _enemiesConfig;
        [SerializeField] private Transform _enemySpawnPointContainer;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_enemyViewPrefab);
            builder.RegisterComponent(_enemiesConfig);
            
            builder.RegisterEntryPoint<EnemyController>().WithParameter(_enemySpawnPointContainer);
        }
    }
}
