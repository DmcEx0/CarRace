using System.Collections.Generic;
using UnityEngine;

namespace CarRace
{
    [CreateAssetMenu(fileName = "WeaponsConfig", menuName = "Configs/Weapons Config")]
    public class WeaponsConfig : ScriptableObject
    {
        [SerializeField] private List<WeaponData> _weaponsData;
        
        public IReadOnlyList<WeaponData> WeaponsData => _weaponsData;
    }
}