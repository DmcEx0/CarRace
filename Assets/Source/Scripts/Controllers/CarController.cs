using System;
using System.Collections.Generic;
using System.Threading;
using CarRace.Configs;
using CarRace.Contexts;
using CarRace.Factory;
using CarRace.Inventory;
using CarRace.Views;
using CarRace.Weapon;
using Cysharp.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;
using VContainer.Unity;

namespace CarRace.Controllers
{
    public class CarController : IAsyncStartable, ITickable, IDisposable
    {
        private readonly WeaponInventorySystem _weaponInventorySystem;
        private readonly WeaponsProvider _weaponsProvider;

        private readonly WeaponFactory _weaponFactory;
        private readonly CarFactory _carFactory;
        
        private readonly TestConfig _testConfig;
        private readonly SceneContext _sceneContext;
        
        private readonly CancellationTokenSource _cts;

        public CarController(WeaponInventorySystem weaponInventorySystem, WeaponFactory weaponFactory, CarFactory carFactory,
            WeaponsProvider weaponsProvider, TestConfig testConfig, SceneContext sceneContext)
        {
            _weaponInventorySystem = weaponInventorySystem;
            _weaponsProvider = weaponsProvider;

            _weaponFactory = weaponFactory;
            _carFactory = carFactory;
            
            _testConfig = testConfig;
            _sceneContext = sceneContext;
            
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

        private async UniTask
            CreateWeaponAsync(WeaponInventoryCell inventoryCell, int slotIndex) //TODO: Для теста, удалить
        {
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

            CameraTarget target = new CameraTarget();
            target.TrackingTarget = view.transform;
            
            _sceneContext.Camera.Target = target;
            
            _weaponsProvider.InitWeaponsSlots(view.WeaponsSlotsTransform);
        }
    }
}