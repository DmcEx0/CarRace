using System.Collections.Generic;
using UnityEngine;

namespace CarRace
{
    [CreateAssetMenu(fileName = "CarConfig", menuName = "Scriptable Objects/CarConfig")]
    public class CarConfig : ScriptableObject
    {
        public Weapon[] EquipedWeapons;

        public void EquipWeapon(int slot, Weapon weapon)
        {
            EquipedWeapons[slot] = weapon;
        }
    }
}
