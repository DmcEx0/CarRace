using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer.Unity;
using Object = UnityEngine.Object;

using CarRace.Gameplay.Configs;
using CarRace.Gameplay.Configs.Test;
using CarRace.Gameplay.Inventory;
using CarRace.UI.Equipment;

namespace CarRace.UI
{
    public class UIController : IStartable, IDisposable
    {
        private readonly TestConfig _testConfig;

        private readonly UIElementsProvider _uiElementsProvider;
        private readonly WeaponInventorySystem _weaponInventorySystem;

        private readonly WeaponsConfig _weaponsConfig;

        private readonly EquipmentCellView _cellViewPrefab;
        
        private List<EquipmentCellView> _spawnedCellsViews;

        public UIController(UIElementsProvider uiElementsProvider, TestConfig testConfig, WeaponsConfig weaponsConfig,
            EquipmentCellView cellViewPrefab, WeaponInventorySystem weaponInventorySystem)
        {
            _uiElementsProvider = uiElementsProvider;
            _testConfig = testConfig;
            _weaponsConfig = weaponsConfig;
            _cellViewPrefab = cellViewPrefab;
            _weaponInventorySystem = weaponInventorySystem;
        }

        public void Start()
        {
            _spawnedCellsViews = new List<EquipmentCellView>();
            
            _uiElementsProvider.EquipmentButton.onClick.AddListener(ShowEquipmentPanel);
            InitCells();
        }

        public void Dispose()
        {
            _uiElementsProvider.EquipmentButton.onClick.RemoveListener(ShowEquipmentPanel);
        }

        private void ShowEquipmentPanel()
        {
            var equipmentView = _uiElementsProvider.EquipmentView;

            equipmentView.gameObject.SetActive(true);
            equipmentView.CloseButton.onClick.AddListener(CloseEquipmentPanel);
        }

        private void CloseEquipmentPanel()
        {
            var equipmentView = _uiElementsProvider.EquipmentView;

            equipmentView.gameObject.SetActive(false);
            equipmentView.CloseButton.onClick.RemoveListener(CloseEquipmentPanel);
        }

        private void InitCells()
        {
            foreach (var initWeaponData in _testConfig.InitialWeapons)
            {
                var cell = new WeaponInventoryCell(initWeaponData.Type, initWeaponData.Level);
                var data = _weaponsConfig.WeaponsData.First(data => data.Type == cell.Type && data.Level == cell.Level);
                var view = Object.Instantiate(_cellViewPrefab, _uiElementsProvider.EquipmentView.InventoryContainer);
                
                view.transform.position = Vector3.zero;
                view.Image.sprite = data.WeaponPreview;
                view.Text.text = data.Level.ToString();
                
                _weaponInventorySystem.Add(cell);
                _spawnedCellsViews.Add(view);
            }
        }
    }
}