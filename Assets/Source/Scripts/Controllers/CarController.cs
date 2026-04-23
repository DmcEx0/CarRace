using System.Collections.Generic;
using CarRace.Inventory;
using CarRace.Views;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;

namespace CarRace.Controllers
{
    public class CarController : IInitializable, IStartable, ITickable
    {
        private readonly WeaponInventorySystem _weaponInventorySystem;
        private readonly WeaponFactory _weaponFactory;
        private readonly CarView _view;

        private List<WeaponSlot> _weaponsSlots;

        private List<KeyValuePair<WeaponType, int>> _initialWeapons; // Для теста

        public CarController(WeaponInventorySystem weaponInventorySystem, WeaponFactory weaponFactory, CarView carView)
        {
            _weaponInventorySystem = weaponInventorySystem;
            _weaponFactory = weaponFactory;
            _view = carView;
        }

        public void Initialize()
        {
            _initialWeapons = new List<KeyValuePair<WeaponType, int>>()
            {
                new (WeaponType.Minigun, 0),
                new (WeaponType.RocketLauncher, 0),
            };

            _weaponsSlots = new List<WeaponSlot>();
        }

        public void Start()
        {
            InitInventory();
            InitWeaponPlaces();
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

        private void InitWeaponPlaces()
        {
            for (int i = 0; i < _view.WeaponsSlotsTransform.Count; i++)
            {
                var weaponPlace = new WeaponSlot(_view.WeaponsSlotsTransform[i], i);
                _weaponsSlots.Add(weaponPlace);
            }
        }

        private async UniTask CreateWeaponAsync() // Test
        {
            for (int i = 0; i < _initialWeapons.Count; i++)
            {
                var weaponContext = await GetCreatedWeaponAsync(_initialWeapons[i].Key, _initialWeapons[i].Value,
                    _weaponsSlots[i].ParentTransform);
                
                _weaponsSlots[i].Replace(weaponContext);
            }
        }

        private async UniTask<WeaponContext> GetCreatedWeaponAsync(WeaponType type, int level, Transform parent)
        {
            var weaponContext = await _weaponFactory.GetAsync(type, level, parent);

            return weaponContext;
        }
    }
}