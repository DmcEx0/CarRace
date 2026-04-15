using System.Collections.Generic;
using CarRace.Helpers;
using CarRace.Utils;
using UnityEngine;

namespace CarRace.Test
{
    public class TestPlayer : MonoBehaviour
    {
        [SerializeField] private WeaponsConfig _weaponsConfig;
        [SerializeField] private int _weaponIndex = -1;

        [Space] [SerializeField] private LayerMask _enemyLayer;
        [SerializeField] private float _radius;

        private TargetSystem<EnemyView> _targetSystem;

        private EnemyView _target;

        private KeyValuePair<WeaponData, BaseWeaponView> _currentWeapon;
        private int _currentWeaponIndex;

        private void Start()
        {
            _targetSystem = new TargetSystem<EnemyView>(_enemyLayer, 10);
        }

        private void Update()
        {
            var hasTarget =
                _targetSystem.TryGetNearest(out var target, transform.position, _radius);

            if (hasTarget == false)
            {
                _target = null;
                GameDebug.Log("No target found");
            }
            else
            {
                _target = target;
                GameDebug.Log("Target found");
            }
        }

        private void SpawnWeapon()
        {
            if (_weaponIndex <= -1 || _currentWeaponIndex == _weaponIndex)
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

            _currentWeapon = new KeyValuePair<WeaponData, BaseWeaponView>(data, weapon);
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