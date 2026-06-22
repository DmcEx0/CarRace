using System;
using System.Collections.Generic;
using CarRace.Configs;
using CarRace.Factory;
using CarRace.Helpers;
using CarRace.Views;
using CarRace.Weapon;
using UnityEngine;
using VContainer;

namespace CarRace.Test
{
    public class CarTest : MonoBehaviour
    {
        [SerializeField, Range(-1, 1)] private int _weaponIndex = -1;

        [Space] [SerializeField] private LayerMask _enemyLayer;
        [SerializeField] private float _radius;

        private WeaponsConfig _weaponsConfig;
        private ProjectilesFactory _projectilesFactory;

        private TargetFinder<EnemyView> _targetFinder;

        private EnemyView _target;

        private KeyValuePair<WeaponData, WeaponView> _currentWeapon;
        private int _currentWeaponIndex;

        private List<BaseProjectileBehaviour> _projectiles;

        private float _timeForSpawnProjectiles;

        [Inject]
        public void Construct(ProjectilesFactory projectilesFactory, WeaponsConfig weaponsConfig)
        {
            _weaponsConfig = weaponsConfig;
            _projectilesFactory = projectilesFactory;
        }

        private void Start()
        {
            _projectiles = new List<BaseProjectileBehaviour>();
            _targetFinder = new TargetFinder<EnemyView>(_enemyLayer, 10);
            _currentWeaponIndex = _weaponIndex;
        }

        private void Update()
        {
            SpawnWeapon();

            _timeForSpawnProjectiles += Time.deltaTime;

            if (_currentWeapon.Key == null)
            {
                return;
            }

            if (TryFindTarget() && _timeForSpawnProjectiles >= _currentWeapon.Key.BaseFireRate)
            {
                _timeForSpawnProjectiles = 0;

                SpawnProjectile();
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

        private bool TryFindTarget()
        {
            var hasTarget =
                _targetFinder.TryGetNearest(transform.position, _radius, out var target);

            if (hasTarget == false)
            {
                _target = null;
                return false;
            }

            _target = target;
            return true;
        }

        private void SpawnProjectile()
        {
            // var projectile = _projectilesFactory.Get(_currentWeapon.Key, _target.transform.position,
            //     _currentWeapon.Value.FirePoints[0].position);
            //
            // projectile.View.DetectedEnemy += OnProjectileEnemyDetected;

            // _projectiles.Add(projectile);
        }

        private void SpawnWeapon()
        {
            if (_weaponIndex <= -1 || _currentWeaponIndex == _weaponIndex ||
                _weaponIndex >= _weaponsConfig.WeaponsData.Count)
            {
                return;
            }

            if (_currentWeapon.Value != null)
            {
                Destroy(_currentWeapon.Value.gameObject);
            }

            _currentWeaponIndex = _weaponIndex;

            var data = _weaponsConfig.WeaponsData[_currentWeaponIndex];

            var weapon = Instantiate(data.WeaponViewPrefab, transform.position + Vector3.up, Quaternion.identity,
                transform);

            _currentWeapon = new KeyValuePair<WeaponData, WeaponView>(data, weapon);
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

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _radius);

            if (_target != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawCube(_target.transform.position, Vector3.one * 1.1f);

                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(transform.position, _target.transform.position);
            }
        }
    }
}