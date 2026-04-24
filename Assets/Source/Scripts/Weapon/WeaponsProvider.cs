using System.Collections.Generic;
using CarRace.Views;
using UnityEngine;

namespace CarRace.Weapon
{
    public class WeaponsProvider
    {
        private readonly List<WeaponSlot> _weaponsSlots;
        
        public IReadOnlyList<WeaponSlot>  WeaponsSlots => _weaponsSlots;

        public WeaponsProvider()
        {
            _weaponsSlots = new List<WeaponSlot>();
        }

        public void InitWeaponsSlots(IReadOnlyList<Transform> transformsSlots)
        {
            for (int i = 0; i < transformsSlots.Count; i++)
            {
                var weaponPlace = new WeaponSlot(transformsSlots[i], i);
                _weaponsSlots.Add(weaponPlace);
            }
        }
    }
}