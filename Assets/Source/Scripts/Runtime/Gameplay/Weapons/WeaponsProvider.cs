using System.Collections.Generic;
using UnityEngine;

namespace CarRace.Gameplay.Weapons
{
    public class WeaponsProvider
    {
        private readonly List<WeaponSlot> _weaponsSlots;
        
        public IReadOnlyList<WeaponSlot>  WeaponsSlots => _weaponsSlots;
        
        public bool IsInitialized {get; private set;}

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

            IsInitialized = true;
        }
    }
}