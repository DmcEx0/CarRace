using System;
using System.Linq;
using CarRace.Factory;
using Cysharp.Threading.Tasks;

namespace CarRace
{
    public class WeaponFactory : GameObjectFactory
    {
        private readonly WeaponsConfig _weaponsConfig;

        public WeaponFactory(WeaponsConfig weaponsConfig)
        {
            _weaponsConfig = weaponsConfig;
        }
        
        public async UniTask<WeaponContext> GetAsync(WeaponType type, int level) // Мб, заменить тип и левел на ID
        {
            var data = _weaponsConfig.WeaponsData.FirstOrDefault(wpn => wpn.Type == type && wpn.Level == level);
            
            if(data == null)
            {
                throw new Exception($"Weapon type: {type}, lvl: {level} not found");
            }
            
            var instance = await CreateWithAddressAsync<WeaponView>(data.Reference);

            var context = new WeaponContext(data, instance.Key, instance.Value);

            return context;
        }
    }
}
