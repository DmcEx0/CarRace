using UnityEngine;
using VContainer;
using VContainer.Unity;

using CarRace.Gameplay.Car;
using CarRace.Gameplay.Configs.Test;

namespace CarRace.Composition.LifetimeScopes
{
    public class TestLifetimeScope : LifetimeScope
    {
        // [SerializeField] private Car _testPlayer;
        // [SerializeField] private CarView _carView;
        [SerializeField] private TestConfig _testConfig;

        protected override void Configure(IContainerBuilder builder)
        {
            // builder.RegisterInstance(_testPlayer);
            // builder.RegisterInstance(_carView);
            
            builder.RegisterComponent(_testConfig);
            
            // builder.RegisterBuildCallback(container =>
            // {
            //     container.Inject(_testPlayer);
            // });
        }
    }
}