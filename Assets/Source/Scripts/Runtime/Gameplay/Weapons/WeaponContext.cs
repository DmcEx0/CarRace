using CarRace.Composition.SceneContexts;
using UnityEngine.AddressableAssets;

using CarRace.Gameplay.Configs;
using CarRace.Gameplay.Weapons.Projectiles.Settings;
using CarRace.Infrastructure.Factories;

namespace CarRace.Gameplay.Weapons
{
    public class WeaponContext
    {
        private readonly WeaponData _data;

        public WeaponView View { get; private set; }
        
        public ProjectilesFactory ProjectilesFactory {get; private set;}

        public WeaponType Type => _data.Type;
        public int Level => _data.Level;
        public float BaseDamage => _data.BaseDamage;
        public float BaseFireRate => _data.BaseFireRate;
        public float BaseRange => _data.BaseRange;
        public AssetReference ProjectileReference => _data.ProjectileReference;
        public ProjectileSettings ProjectileSettings => _data.ProjectileSettings;

        public WeaponContext(WeaponData data, WeaponView view, BootstrapSceneContext sceneContext)
        {
            _data = data;
            View = view;

            ProjectilesFactory = new ProjectilesFactory(sceneContext);
        }
    }
}