using System.Collections.Generic;
using CarRace.Inventory;
using CarRace.Views;
using CarRace.Weapon;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;

namespace CarRace.Controllers
{
    public class CarController : IInitializable, IStartable, ITickable
    {
        private readonly WeaponInventorySystem _weaponInventorySystem;
        private readonly WeaponFactory _weaponFactory;
        private readonly WeaponsProvider _weaponsProvider;
        private readonly CarView _view;

        private List<KeyValuePair<WeaponType, int>> _initialWeapons; // Для теста

        public CarController(WeaponInventorySystem weaponInventorySystem, WeaponFactory weaponFactory, CarView carView,
            WeaponsProvider weaponsProvider)
        {
            _weaponInventorySystem = weaponInventorySystem;
            _weaponFactory = weaponFactory;
            _weaponsProvider = weaponsProvider;
            _view = carView;
        }

        public void Initialize()
        {
            _initialWeapons = new List<KeyValuePair<WeaponType, int>>()
            {
                new(WeaponType.Minigun, 0),
                new(WeaponType.RocketLauncher, 0),
            };
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
                CreateWeaponAsync().Forget();
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

        private async UniTask CreateWeaponAsync() // Test
        {
            for (int i = 0; i < _initialWeapons.Count; i++)
            {
                var weaponContext = await GetCreatedWeaponAsync(_initialWeapons[i].Key, _initialWeapons[i].Value,
                    _weaponsProvider.WeaponsSlots[i].ParentTransform);
                
                _weaponsProvider.WeaponsSlots[i].Replace(weaponContext);
            }
        }

        private async UniTask<WeaponContext> GetCreatedWeaponAsync(WeaponType type, int level, Transform parent)
        {
            var weaponContext = await _weaponFactory.GetAsync(type, level, parent);

            return weaponContext;
        }
    }
}