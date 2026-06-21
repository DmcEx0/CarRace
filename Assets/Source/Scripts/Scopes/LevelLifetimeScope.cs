using CarRace.Configs;
using CarRace.Contexts;
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