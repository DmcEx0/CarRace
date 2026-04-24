using System;
using System.Collections.Generic;
using System.Linq;
using CarRace.Helpers;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace CarRace
{
    public class Car : MonoBehaviour
    {
        [SerializeField] private Transform[] _weaponSlots;
        [SerializeField] private LayerMask _enemyLayer;
        
        private KeyValuePair<WeaponData, WeaponView>[] _instancedWeapons;
        private WeaponsConfig _weaponsConfig;
        private ProjectilesFactory _projectilesFactory;
        
        private List<BaseProjectileBehaviour> _projectiles;
        private TargetSystem<EnemyView> _targetSystem;

        [Inject]
        public void Construct(ProjectilesFactory projectilesFactory, WeaponsConfig weaponsConfig)
        {
            _weaponsConfig = weaponsConfig;
            _projectilesFactory = projectilesFactory;
        }

        void Start()
        {
            int i = 0;
            _instancedWeapons = new KeyValuePair<WeaponData, WeaponView>[_weaponSlots.Count()];
            
            foreach (var weapon in _weaponsConfig.WeaponsData)
            {
                EquipWeapon(i, weapon);
                i++;
            }
            
            _projectiles = new List<BaseProjectileBehaviour>();
            _targetSystem = new TargetSystem<EnemyView>(_enemyLayer, 10);
            
            foreach (var weapon in _instancedWeapons)
            {
                FireWeapon(weapon.Key, weapon.Value).Forget();
            }
        }

        private void FixedUpdate()
        {
            UpdateProjectiles(Time.fixedDeltaTime);
        }

        private void UpdateProjectiles(float deltaTime)
        {
            foreach (var projectile in _projectiles)
            {
                projectile.OnMove(deltaTime);
            }
        }
        
        public void EquipWeapon(int slot, WeaponData weapon)
        {
            if (weapon != null)
            {
                if (_weaponSlots[slot].childCount > 0)
                {
                    var child = _weaponSlots[slot].GetChild(0);
                    if (child != null)
                        Destroy(child);
                }

                var inst = Instantiate(weapon.WeaponViewPrefab, _weaponSlots[slot]);
                _instancedWeapons[slot] = new(weapon, inst);
            }
        }

        private async UniTask FireWeapon(WeaponData data, WeaponView instance) //TODO: добавить токен отмены
        {
            while (true)
            {
                var hasTarget =
                    _targetSystem.TryGetNearest(out var target, instance.transform.position, data.BaseRange);
                
                if (hasTarget)
                {
                    // var projectile = _projectilesFactory.Get(data, target.transform.position,
                    // instance.FirePoints[0].position);
                    //
                    // projectile.View.DetectedEnemy += OnProjectileEnemyDetected;

                    // _projectiles.Add(projectile);
                }

                await UniTask.Delay(TimeSpan.FromSeconds(data.BaseFireRate));
            }
        }

        private void OnProjectileEnemyDetected(EnemyView enemyView, ProjectileView projectileView)
        {
            // projectileView.DetectedEnemy -= OnProjectileEnemyDetected;

            var projectile = _projectiles.Find(prj => prj.View == projectileView);

            if (projectile == null)
            {
                return;
            }

            _projectiles.Remove(projectile);
            Destroy(projectileView.gameObject);
        }
    }
}
