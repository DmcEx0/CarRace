using CarRace.Test;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace CarRace
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private EnemyView _enemyViewPrefab;
        [SerializeField] private EnemiesConfig _enemiesConfig;
        [SerializeField] private WeaponsConfig _weaponsConfig;
        [SerializeField] private Transform _enemySpawnPointContainer;
        
        //For Test
        [SerializeField] private TestPlayer _testPlayer;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_playerTransform).Keyed(TransformKey.PlayerTransform);
            
            builder.RegisterComponent(_enemiesConfig);
            builder.RegisterComponent(_weaponsConfig);
            builder.RegisterComponent(_enemyViewPrefab);

            builder.Register<ProjectilesFactory>(Lifetime.Singleton);
            
            builder.Register<IdleState>(Lifetime.Transient);
            builder.Register<FollowState>(Lifetime.Transient);
            builder.Register<AttackState>(Lifetime.Transient);
            builder.Register<DieState>(Lifetime.Transient);
            builder.RegisterEntryPoint<StateMachine>().AsSelf();

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

