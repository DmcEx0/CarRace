using CarRace.Test;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace CarRace
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private EnemyView _enemyViewPrefab;
        [SerializeField] private EnemiesConfig _enemiesConfig;
        [SerializeField] private WeaponsConfig _weaponsConfig;
        [SerializeField] private Transform _enemySpawnPointContainer;
        
        //For Test
        [SerializeField] private TestPlayer _testPlayer;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_enemyViewPrefab);
            builder.RegisterComponent(_enemiesConfig);
            builder.RegisterComponent(_weaponsConfig);
            
            builder.Register<ProjectilesFactory>(Lifetime.Singleton);
            
            builder.RegisterEntryPoint<EnemyController>().WithParameter(_enemySpawnPointContainer);
            
            TestConfigure(builder);
        }
        
        private void TestConfigure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_testPlayer);
            
            builder.RegisterBuildCallback(container =>
            {
                container.Inject(_testPlayer);
            });
        }
    }
}
