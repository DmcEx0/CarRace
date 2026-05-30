using System.Collections.Generic;
using CarRace.Configs;
using CarRace.Views;
using UnityEngine;

namespace CarRace.PlaceHolders
{
    public class GarageCar : MonoBehaviour
    {
        [SerializeField] private Transform[] _weaponSlots;
        private KeyValuePair<WeaponData, WeaponView>[] _instancedWeapons;

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
    }
}
