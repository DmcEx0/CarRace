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
    public class CarController : IInitializable, IStartable, ITickable, IDisposable
    {
        private readonly WeaponInventorySystem _weaponInventorySystem;
        private readonly WeaponFactory _weaponFactory;
        private readonly WeaponsProvider _weaponsProvider;
        private readonly CarView _view;

        private CancellationTokenSource _ctx;

        private List<KeyValuePair<WeaponType, int>> _initialWeapons; // Для теста

        public CarController(WeaponInventorySystem weaponInventorySystem, WeaponFactory weaponFactory, CarView carView,
            WeaponsProvider weaponsProvider)
        {
            _weaponInventorySystem = weaponInventorySystem;
            _weaponFactory = weaponFactory;
            _weaponsProvider = weaponsProvider;
            _view = carView;
            
            _ctx = new CancellationTokenSource();
        }

        public void Initialize()
        {
            _initialWeapons = new List<KeyValuePair<WeaponType, int>>()
            {
                new(WeaponType.Minigun, 0),
                new(WeaponType.RocketLauncher, 0),
                new(WeaponType.Minigun, 1),
                new(WeaponType.RocketLauncher, 1),
            };
        }
        
        public void Dispose()
        {
            _ctx?.Cancel();
            _ctx?.Dispose();
        }

        public void Start()
        {
            InitInventory();
            _weaponsProvider.InitWeaponsSlots(_view.WeaponsSlotsTransform);
        }

        public void Tick() // Для теста
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                CreateWeaponAsync(WeaponType.Minigun, 0).Forget();
            }
            else if (Input.GetKeyDown(KeyCode.O))
            {
                CreateWeaponAsync(WeaponType.RocketLauncher, 0).Forget();
            }
        }

        private void InitInventory()
        {
            foreach (var initWeapon in _initialWeapons)
            {
                var cell = new WeaponInventoryCell(initWeapon.Key, initWeapon.Value);
                _weaponInventorySystem.Add(cell);
            }
        }

        private async UniTask CreateWeaponAsync(WeaponType type, int lvl) // Test
        {
            for (int i = 0; i < _weaponsProvider.WeaponsSlots.Count; i++)
            {
                var slot = _weaponsProvider.WeaponsSlots[i];
                
                var weaponContext = await GetCreatedWeaponAsync(type, lvl, slot.ParentTransform);
                
                slot.Replace(weaponContext);
            }
        }

        private async UniTask<WeaponContext> GetCreatedWeaponAsync(WeaponType type, int level, Transform parent)
        {
            var weaponContext = await _weaponFactory.GetAsync(type, level, parent, _ctx.Token);

            return weaponContext;
        }
    }
}