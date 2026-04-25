using System;
using VContainer.Unity;

namespace CarRace.UI
{
    public class UIController : IStartable, IDisposable
    {
        private readonly UIElementsProvider _uiElementsProvider;

        public UIController(UIElementsProvider uiElementsProvider)
        {
            _uiElementsProvider = uiElementsProvider;
        }

        public void Start()
        {
            _uiElementsProvider.EquipmentButton.onClick.AddListener(ShowEquipmentPanel);
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
    }
}
