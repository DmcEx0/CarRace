using System;
using System.Linq;
using System.Threading;
using CarRace.Configs;
using CarRace.Contexts;
using CarRace.Inventory;
using CarRace.Views;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CarRace.Factory
{
    public class WeaponFactory : GameObjectFactory
    {
        private readonly WeaponsConfig _weaponsConfig;

        public WeaponFactory(WeaponsConfig weaponsConfig)
        {
            _weaponsConfig = weaponsConfig;
        }

        public async UniTask<WeaponContext> GetAsync(WeaponInventoryCell inventoryCell, Transform parent,
            CancellationToken token) // TODO: Мб, заменить тип и левел на ID
        {
            var data = _weaponsConfig.WeaponsData.FirstOrDefault(wpn =>
                wpn.Type == inventoryCell.Type && wpn.Level == inventoryCell.Level);

            if (data == null)
            {
                throw new Exception($"Weapon type: {inventoryCell.Type}, lvl: {inventoryCell.Level} not found");
            }

            var spawnResult = await CreateWithAddressAsync<WeaponView>(data.WeaponReference, token);
            
            var instance = Create(spawnResult.Prefab, parent);

            instance.transform.localPosition = Vector3.zero;
            instance.transform.localRotation = Quaternion.identity;

            var context = new WeaponContext(data, instance, spawnResult);

            return context;
        }
    }
}