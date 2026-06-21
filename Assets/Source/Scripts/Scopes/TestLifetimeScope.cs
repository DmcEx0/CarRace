using CarRace.Configs;
using CarRace.Placeholders;
using CarRace.Views;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace CarRace.Scopes
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