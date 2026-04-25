using CarRace.Controllers;
using CarRace.Inventory;
using CarRace.UI;
using CarRace.Views;
using CarRace.Weapon;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace CarRace
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private EnemiesConfig _enemiesConfig;
        [SerializeField] private WeaponsConfig _weaponsConfig;
        [SerializeField] private GameConfig _gameConfig;
        
        [Space]
        [SerializeField] private Transform _enemySpawnPointContainer;
        [SerializeField] private Transform _enemyPoolContainer;
        [SerializeField] private Transform _projectilePoolContainer;

        [Space]
        [SerializeField] private UIElementsProvider _uiElementsProvider;
        
        //For Test
        [Space]
        [SerializeField] private Car _testPlayer;
        [SerializeField] private CarView _carView;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_testPlayer);
            builder.RegisterInstance(_carView);
            builder.RegisterInstance(_uiElementsProvider);

            builder.RegisterComponent(_enemiesConfig);
            builder.RegisterComponent(_weaponsConfig);
            builder.RegisterComponent(_gameConfig);

            builder.Register<WeaponFactory>(Lifetime.Singleton);
            
            builder.Register<ProjectilesFactory>(Lifetime.Singleton).WithParameter(_projectilePoolContainer);
            builder.Register<EnemyFactory>(Lifetime.Singleton).WithParameter(_enemyPoolContainer);
            
            builder.Register<WeaponsProvider>(Lifetime.Singleton);
            
            builder.Register<WeaponInventorySystem>(Lifetime.Singleton);

            builder.RegisterEntryPoint<EnemyController>().WithParameter(_enemySpawnPointContainer);
            builder.RegisterEntryPoint<CarController>();
            builder.RegisterEntryPoint<WeaponsController>();
            builder.RegisterEntryPoint<UIController>();

            // TestConfigure(builder);
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



