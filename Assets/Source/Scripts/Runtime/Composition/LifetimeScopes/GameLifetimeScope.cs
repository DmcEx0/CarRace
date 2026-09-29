using UnityEngine;
using VContainer;
using VContainer.Unity;

using CarRace.Composition.SceneContexts;
using CarRace.Gameplay.Car;
using CarRace.Gameplay.Configs;
using CarRace.Gameplay.Enemies;
using CarRace.Gameplay.Inventory;
using CarRace.Gameplay.Weapons;
using CarRace.Infrastructure.Factories;
using CarRace.Infrastructure.Services;
using CarRace.UI;
using CarRace.UI.Equipment;

namespace CarRace.Composition.LifetimeScopes
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private WeaponsConfig _weaponsConfig;
        [SerializeField] private GameConfig _gameConfig;
        
        [Space]
        [SerializeField] private BootstrapSceneContext _bootstrapSceneContext;

        [Space]
        [SerializeField] private UIElementsProvider _uiElementsProvider;
        [SerializeField] private EquipmentCellView _equipmentCellView;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_bootstrapSceneContext);
            builder.RegisterInstance(_uiElementsProvider);

            builder.RegisterComponent(_weaponsConfig);
            builder.RegisterComponent(_gameConfig);
            builder.RegisterComponent(_equipmentCellView);

            builder.Register<WeaponFactory>(Lifetime.Singleton);
            builder.Register<CarFactory>(Lifetime.Singleton);
            builder.Register<PlayerCarModel>(Lifetime.Singleton);
            builder.Register<WeaponsProvider>(Lifetime.Singleton);
            builder.Register<WeaponInventorySystem>(Lifetime.Singleton);
            builder.Register<SceneLoadingService>(Lifetime.Singleton);

            builder.RegisterEntryPoint<GameInitializeService>();
            builder.RegisterEntryPoint<PlayerCarController>();
            builder.RegisterEntryPoint<WeaponsController>();
            builder.RegisterEntryPoint<UIController>();
        }
    }
}