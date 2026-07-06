using System;
using System.Linq;
using System.Threading;
using CarRace.Composition.SceneContexts;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;
using CarRace.Gameplay.Configs;
using CarRace.Gameplay.Inventory;
using CarRace.Gameplay.Weapons;

namespace CarRace.Infrastructure.Factories
{
    public class WeaponFactory : GameObjectFactory, IDisposable
    {
        private readonly WeaponsConfig _weaponsConfig;
        private readonly BootstrapSceneContext _sceneContext;

        private SpawnResult<WeaponView> _spawnResult;

        public WeaponFactory(WeaponsConfig weaponsConfig, BootstrapSceneContext sceneContext)
        {
            _weaponsConfig = weaponsConfig;
            _sceneContext = sceneContext;
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

            _spawnResult = await CreateWithAddressAsync<WeaponView>(data.WeaponReference, token);

            var instance = Create(_spawnResult.Prefab, parent);

            instance.transform.localPosition = Vector3.zero;
            instance.transform.localRotation = Quaternion.identity;

            var context = new WeaponContext(data, instance, _sceneContext);

            return context;
        }

        public void Dispose()
        {
            if (_spawnResult != null)
            {
                _spawnResult.Release();
            }
        }
    }
}