using UnityEngine;
using VContainer;
using VContainer.Unity;

using CarRace.Composition.SceneContexts;
using CarRace.Gameplay.Configs;
using CarRace.Gameplay.Enemies;
using CarRace.Infrastructure.Factories;

namespace CarRace.Composition.LifetimeScopes
{
    public class LevelLifetimeScope : LifetimeScope
    {
        [SerializeField] private EnemiesConfig _enemiesConfig;
        [SerializeField] private LevelSceneContext _levelSceneContext;
        
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_levelSceneContext);
            
            builder.RegisterComponent(_enemiesConfig);
            
            builder.Register<EnemyFactory>(Lifetime.Singleton);
            
            builder.RegisterEntryPoint<EnemyController>();
        }
    }
}