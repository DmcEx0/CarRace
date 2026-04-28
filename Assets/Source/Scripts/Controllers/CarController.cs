using System;
using System.Collections.Generic;
using System.Threading;
using CarRace.Inventory;
using CarRace.Views;
using CarRace.Weapon;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;

namespace CarRace.Controllers
{
    public class CarController : IStartable, ITickable, IDisposable
    {
        private readonly WeaponInventorySystem _weaponInventorySystem;
        private readonly WeaponFactory _weaponFactory;
        private readonly WeaponsProvider _weaponsProvider;
        private readonly CarView _view;

        private readonly CancellationTokenSource _ctx;

        public CarController(WeaponInventorySystem weaponInventorySystem, WeaponFactory weaponFactory, CarView carView,
            WeaponsProvider weaponsProvider)
        {
            _weaponInventorySystem = weaponInventorySystem;
            _weaponFactory = weaponFactory;
            _weaponsProvider = weaponsProvider;
            _view = carView;

            _ctx = new CancellationTokenSource();
        }

        public void Dispose()
        {
            _ctx?.Cancel();
            _ctx?.Dispose();
        }

        public void Start()
        {
            _weaponsProvider.InitWeaponsSlots(_view.WeaponsSlotsTransform);
        }

        public void Tick() // Для теста
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

        private async UniTask CreateWeaponAsync(WeaponInventoryCell inventoryCell, int slotIndex) // Test
        {
            var slot = _weaponsProvider.WeaponsSlots[slotIndex];

            var weaponContext = await GetCreatedWeaponAsync(inventoryCell, slot.ParentTransform);

            slot.Replace(weaponContext);
        }

        private async UniTask<WeaponContext> GetCreatedWeaponAsync(WeaponInventoryCell inventoryCell, Transform parent)
        {
            var weaponContext = await _weaponFactory.GetAsync(inventoryCell, parent, _ctx.Token);

            return weaponContext;
        }
    }
}