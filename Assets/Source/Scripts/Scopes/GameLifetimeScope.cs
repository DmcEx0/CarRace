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
        [SerializeField] private Transform _enemySpawnPointContainer;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_playerTransform).Keyed(TransformKey.PlayerTransform);
            
            builder.RegisterComponent(_enemiesConfig);

            builder.Register<IdleState>(Lifetime.Transient);
            builder.Register<FollowState>(Lifetime.Transient);
            builder.Register<AttackState>(Lifetime.Transient);
            builder.Register<DieState>(Lifetime.Transient);
            builder.RegisterEntryPoint<StateMachine>().AsSelf();

            builder.RegisterComponent(_enemyViewPrefab);

            builder.RegisterEntryPoint<EnemyController>().WithParameter(_enemySpawnPointContainer);
            
        }
    }

    public enum TransformKey

    {
        PlayerTransform,
        EnemySpawnPointTransform
    }
}

