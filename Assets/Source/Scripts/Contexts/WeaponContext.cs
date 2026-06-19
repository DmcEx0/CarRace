using CarRace.Configs;
using CarRace.Factory;
using CarRace.Views;
using CarRace.Weapon;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CarRace.Contexts
{
    public class WeaponContext
    {
        private readonly WeaponData _data;

        public WeaponView View { get; private set; }
        
        public ProjectilesFactory ProjectilesFactory {get; private set;}
        public SpawnResult<WeaponView> SpawnResult {get; private set;}

        public WeaponType Type => _data.Type;
        public int Level => _data.Level;
        public float BaseDamage => _data.BaseDamage;
        public float BaseFireRate => _data.BaseFireRate;
        public float BaseRange => _data.BaseRange;
        public AssetReference ProjectileReference => _data.ProjectileReference;
        public ProjectileSettings ProjectileSettings => _data.ProjectileSettings;

        public WeaponContext(WeaponData data, WeaponView view, SpawnResult<WeaponView> spawnResult)
        {
            _data = data;
            View = view;
            SpawnResult = spawnResult;

            ProjectilesFactory = new ProjectilesFactory(null);
        }
    }
}