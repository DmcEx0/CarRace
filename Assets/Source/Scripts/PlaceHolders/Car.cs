using UnityEngine;

namespace CarRace
{
    public class Car : MonoBehaviour
    {
        [SerializeField] private Transform[] _weaponSlots;
        [SerializeField] private CarConfig carConfig;
        
        void Start()
        {
            
            int i = 0;
            foreach (var weapon in carConfig.EquipedWeapons)
            {
                EquipWeapon(i, weapon);
                i++;
            }
        }

        public void EquipWeapon(int slot, Weapon weapon)
        {
            if (weapon.WeaponPrefab != null)
            {
                var child = _weaponSlots[slot].GetChild(0);
                if (child != null)
                    Destroy(child);
                    
                Instantiate(weapon.WeaponPrefab, _weaponSlots[slot]);
            }
                
        }
        
    }
}
