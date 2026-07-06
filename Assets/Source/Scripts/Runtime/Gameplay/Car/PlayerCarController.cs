using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;
using VContainer.Unity;
using CarRace.Composition.SceneContexts;
using CarRace.Gameplay.Configs.Test;
using CarRace.Gameplay.Inventory;
using CarRace.Gameplay.Weapons;
using CarRace.Infrastructure.Factories;

namespace CarRace.Gameplay.Car
{
    public class PlayerCarController : IAsyncStartable, ITickable, IDisposable
    {
        private readonly WeaponInventorySystem _weaponInventorySystem;
        private readonly WeaponsProvider _weaponsProvider;

        private readonly WeaponFactory _weaponFactory;
        private readonly CarFactory _carFactory;

        private readonly PlayerCarModel _model;

        private readonly TestConfig _testConfig;
        private readonly BootstrapSceneContext _bootstrapSceneContext;

        private readonly CancellationTokenSource _cts;

        public PlayerCarController(WeaponInventorySystem weaponInventorySystem, WeaponFactory weaponFactory,
            CarFactory carFactory, PlayerCarModel model, WeaponsProvider weaponsProvider, TestConfig testConfig,
            BootstrapSceneContext bootstrapSceneContext)
        {
            _weaponInventorySystem = weaponInventorySystem;
            _weaponsProvider = weaponsProvider;

            _weaponFactory = weaponFactory;
            _carFactory = carFactory;
            
            _model = model;

            _testConfig = testConfig;
            _bootstrapSceneContext = bootstrapSceneContext;

            _cts = new CancellationTokenSource();
        }

        public void Dispose()
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }

        public async UniTask StartAsync(CancellationToken cancellation = new CancellationToken())
        {
            await CreateCarAsync();
        }

        public void Tick() //TODO: Для теста, удалить
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                CreateWeaponAsync(_weaponInventorySystem.Cells[0], 0).Forget();
                CreateWeaponAsync(_weaponInventorySystem.Cells[1], 1).Forget();
            }
            else if (Input.GetKeyDown(KeyCode.O))
            {
                CreateWeaponAsync(_weaponInventorySystem.Cells[2], 0).Forget();
                CreateWeaponAsync(_weaponInventorySystem.Cells[3], 1).Forget();
            }
        }

        private async UniTask CreateWeaponAsync(WeaponInventoryCell inventoryCell, int slotIndex) //TODO: Для теста, удалить
        {
            _weaponFactory.Dispose();
            
            var slot = _weaponsProvider.WeaponsSlots[slotIndex];

            var weaponContext = await GetCreatedWeaponAsync(inventoryCell, slot.ParentTransform);

            slot.Replace(weaponContext);
        }

        private async UniTask<WeaponContext> GetCreatedWeaponAsync(WeaponInventoryCell inventoryCell, Transform parent)
        {
            var weaponContext = await _weaponFactory.GetAsync(inventoryCell, parent, _cts.Token);

            return weaponContext;
        }

        private async UniTask CreateCarAsync()
        {
            var view = await _carFactory.GetAsync(_testConfig.CarReference, _cts.Token);
            var context = new PlayerCarContext(view);

            _model.SetContext(context);
            
            CameraTarget target = new CameraTarget
            {
                TrackingTarget = view.transform
            };

            _bootstrapSceneContext.Camera.Target = target;

            _weaponsProvider.InitWeaponsSlots(view.WeaponsSlotsTransform);
        }
    }
}