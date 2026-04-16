using CarRace.Factory;
using UnityEngine;

namespace CarRace
{
    public class ProjectilesFactory : GameObjectFactory
    {
        public BaseProjectile Get(WeaponData weaponData, Vector3 targetPosition, Vector3 position)
        {
            BaseProjectile projectile = default;

            var instance = Object.Instantiate(weaponData.ProjectileViewPrefab, position,
                Quaternion.identity);

            switch (weaponData.Type)
            {
                case WeaponType.Minigun:
                    projectile = new ForwardProjectile(instance, weaponData.ProjectileSpeed, targetPosition);
                    break;
                case WeaponType.RocketLauncher:
                    projectile = new BallisticProjectile(instance, weaponData.ProjectileSpeed, targetPosition);
                    break;
            }

            return projectile;
        }
    }
}