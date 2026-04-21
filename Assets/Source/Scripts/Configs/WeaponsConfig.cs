using System.Collections.Generic;
using UnityEngine;

namespace CarRace
{
    [CreateAssetMenu(fileName = "WeaponsConfig", menuName = "Configs/Weapons Config")]
    public class WeaponsConfig : ScriptableObject
    {
        [SerializeField] private WeaponData[] _weaponsData;

        public IReadOnlyList<WeaponData> WeaponsData => _weaponsData;
        
        public void SetWeapon(int slot, WeaponData weapon)
        {
            _weaponsData[slot] = weapon;
        }
    }
}