using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CarRace
{
    public class WeaponPickerController : MonoBehaviour
    {
        private int _activeSlot;
        [SerializeField] private OwnedWeapons _ownedWeapons;
        [SerializeField] private WeaponsConfig _weaponsConfig;
        [SerializeField] private RectTransform _contentTransfrom;
        [SerializeField] private Button _buttonPrefab;
        [SerializeField] private List<Button> _slotsButtons;
        private Car car;
        void Awake()
        {
            car = FindAnyObjectByType<Car>();
            for (int k = 0; k < _weaponsConfig.WeaponsData.Count; k++)
            {
                _slotsButtons[k].SetActive(true);
            }
            
            int i = 0;
            foreach (var weapon in _ownedWeapons.Weapons)
            {
                i++;
                var b = Instantiate(_buttonPrefab, _contentTransfrom);
                b.GetComponent<RectTransform>().anchoredPosition = new Vector3(i * 200, 0, 0);
                b.GetComponent<Image>().sprite = weapon.WeaponPreview;
                b.onClick.AddListener(() => IntstallWeapon(weapon.data));

            }
        }
        
        public void ChangeActiveSlot(int slot)
        {
            _activeSlot = slot;
        }
        public void IntstallWeapon(WeaponData weapon)
        {
            _weaponsConfig.SetWeapon(_activeSlot, weapon);
            car.EquipWeapon(_activeSlot, weapon);
        }


    }
}
