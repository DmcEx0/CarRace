using CarRace.Configs;
using UnityEngine;
using UnityEngine.UI;
using VContainer.Unity;

namespace CarRace.Placeholders
{
    public class CanvasController : IInitializable
    {
        private readonly GarageCar _car;
        private readonly CanvasView _view;
        private readonly WeaponsConfig _weaponsConfig;
        
        private int _activeSlot;
        public CanvasController(CanvasView view, GarageCar car, WeaponsConfig weapons)
        {
            _view = view;
            _car = car;
            _weaponsConfig = weapons;
        }

        public void Initialize()
        {

            for (int i = 0; i < _weaponsConfig.WeaponsData.Count; i++)
            {
                var data = _weaponsConfig.WeaponsData[i];
                
                _view.SlotsButtons[i].SetActive(true);
                _view.SlotsButtons[i].onClick.AddListener(() => ChangeActiveSlot(i));
                
                var b = Object.Instantiate(_view.ButtonPrefab, _view.ContentTransfrom);
                b.GetComponent<RectTransform>().anchoredPosition = new Vector3(i * 200, 0, 0);
                b.GetComponent<Image>().sprite = data.WeaponPreview;
                b.onClick.AddListener(() => IntstallWeapon(data));
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

