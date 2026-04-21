using CarRace.Test;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace CarRace
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private EnemiesConfig _enemiesConfig;
        [SerializeField] private WeaponsConfig _weaponsConfig;
        
        [SerializeField] private Transform _enemySpawnPointContainer;
        [SerializeField] private Transform _enemyPoolContainer;
        
        //For Test
        [SerializeField] private Car _testPlayer;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_playerTransform).Keyed(TransformKey.PlayerTransform);
            
            builder.RegisterComponent(_enemiesConfig);
            builder.RegisterComponent(_weaponsConfig);

            builder.Register<ProjectilesFactory>(Lifetime.Singleton);
            builder.Register<EnemyFactory>(Lifetime.Singleton).WithParameter(_enemyPoolContainer);

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

    public enum TransformKey

    {
        PlayerTransform,
        EnemySpawnPointTransform
    }
}

