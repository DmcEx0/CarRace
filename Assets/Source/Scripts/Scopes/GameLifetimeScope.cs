using CarRace.Configs;
using CarRace.Controllers;
using CarRace.Factory;
using CarRace.Inventory;
using CarRace.Services;
using CarRace.UI;
using CarRace.Weapon;
using Unity.Cinemachine;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace CarRace.Scopes
{
    public class GameLifetimeScope : LifetimeScope
    {
        // [SerializeField] private EnemiesConfig _enemiesConfig;
        [SerializeField] private WeaponsConfig _weaponsConfig;
        [SerializeField] private GameConfig _gameConfig;
        
        [Space]
        // [SerializeField] private Transform _enemySpawnPointContainer;
        // [SerializeField] private Transform _enemyPoolContainer;
        [SerializeField] private CinemachineCamera _cinemachine;
        [SerializeField] private Transform _projectilePoolContainer;

        [Space]
        [SerializeField] private UIElementsProvider _uiElementsProvider;
        [SerializeField] private EquipmentCellView _equipmentCellView;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_cinemachine);
            builder.RegisterInstance(_uiElementsProvider);

            // builder.RegisterComponent(_enemiesConfig);
            builder.RegisterComponent(_weaponsConfig);
            builder.RegisterComponent(_gameConfig);
            
            builder.RegisterComponent(_equipmentCellView);

            builder.Register<WeaponFactory>(Lifetime.Singleton);
            builder.Register<ProjectilesFactory>(Lifetime.Singleton).WithParameter(_projectilePoolContainer);
            // builder.Register<EnemyFactory>(Lifetime.Singleton).WithParameter(_enemyPoolContainer);
            
            builder.Register<WeaponsProvider>(Lifetime.Singleton);
            
            builder.Register<WeaponInventorySystem>(Lifetime.Singleton);
            
            builder.Register<SceneLoadingService>(Lifetime.Singleton);

            // builder.RegisterEntryPoint<EnemyController>().WithParameter(_enemySpawnPointContainer);
            builder.RegisterEntryPoint<GameInitializeService>();
            
            builder.RegisterEntryPoint<CarController>();
            builder.RegisterEntryPoint<WeaponsController>();
            builder.RegisterEntryPoint<UIController>();
        }
    }
}