using System;
using System.Linq;
using System.Threading;
using CarRace.Configs;
using CarRace.Factory;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CarRace
{
    public class WeaponFactory : GameObjectFactory
    {
        private readonly WeaponsConfig _weaponsConfig;

        public WeaponFactory(WeaponsConfig weaponsConfig)
        {
            _weaponsConfig = weaponsConfig;
        }

        public async UniTask<WeaponContext> GetAsync(WeaponType type, int level, Transform parent,
            CancellationToken token) // Мб, заменить тип и левел на ID
        {
            var data = _weaponsConfig.WeaponsData.FirstOrDefault(wpn => wpn.Type == type && wpn.Level == level);

            if (data == null)
            {
                throw new Exception($"Weapon type: {type}, lvl: {level} not found");
            }

            var result = await CreateWithAddressAsync<WeaponView>(data.WeaponReference, token);

            result.Instance.transform.parent = parent;
            result.Instance.transform.localPosition = Vector3.zero;
            result.Instance.transform.localRotation = Quaternion.identity;

            var context = new WeaponContext(data, result.Instance, result.Handle);

            return context;
        }
    }
}