using CarRace.Configs;
using CarRace.Controllers;
using CarRace.Factory;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace CarRace.Scopes
{
    public class LevelLifetimeScope : LifetimeScope
    {
        [SerializeField] private EnemiesConfig _enemiesConfig;
        [SerializeField] private Transform _enemySpawnPointContainer;
        [SerializeField] private Transform _enemyPoolContainer;
        
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_enemiesConfig);
            
            builder.Register<EnemyFactory>(Lifetime.Singleton).WithParameter(_enemyPoolContainer);
            
            builder.RegisterEntryPoint<EnemyController>().WithParameter(_enemySpawnPointContainer);
        }
    }
}