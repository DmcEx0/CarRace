using UnityEngine;
using UnityEngine.UI;
using VContainer.Unity;

namespace CarRace
{
    public class CanvasController : IInitializable
    {
        private int _activeSlot;
        private GarageCar _car;
        private CanvasView _view;
        private OwnedWeapons _ownedWeapons;
        private WeaponsConfig _weaponsConfig;
        public CanvasController(CanvasView view, OwnedWeapons ownedWeapons, GarageCar car, WeaponsConfig weapons)
        {
            _ownedWeapons = ownedWeapons;
            _view = view;
            _car = car;
            _weaponsConfig = weapons;
        }

        public void Initialize()
        {

            for (int k = 0; k < _weaponsConfig.WeaponsData.Count; k++)
            {
                _view.SlotsButtons[k].SetActive(true);
                _view.SlotsButtons[k].onClick.AddListener(() => ChangeActiveSlot(k));
            }

            int i = 0;
            foreach (var weapon in _ownedWeapons.Weapons)
            {
                i++;
                var b = MonoBehaviour.Instantiate(_view.ButtonPrefab, _view.ContentTransfrom);
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
            /* _weaponsConfig.SetWeapon(_activeSlot, weapon); */
            _car.EquipWeapon(_activeSlot, weapon);
        }
    }
}

