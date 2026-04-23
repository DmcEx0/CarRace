using System.Collections.Generic;
using CarRace.Inventory;
using VContainer.Unity;

namespace CarRace.Controllers
{
    public class CarController : IInitializable, IStartable
    {
        private readonly WeaponInventorySystem _weaponInventorySystem;
        private readonly WeaponFactory _weaponFactory;
        
        private Dictionary<WeaponType, int> _initialWeapons; // Для теста
        
        public CarController(WeaponInventorySystem weaponInventorySystem, WeaponFactory weaponFactory)
        {
            _weaponInventorySystem = weaponInventorySystem;
            _weaponFactory = weaponFactory;
        }

        public void Initialize()
        {
            _initialWeapons = new Dictionary<WeaponType, int>()
            {
                { WeaponType.Minigun, 0 },
                { WeaponType.RocketLauncher, 0 },
            };
        }

        public void Start()
        {
            foreach (var weapon in _initialWeapons)
            {
            }
        }
    }
}
