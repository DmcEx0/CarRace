using UnityEngine;
using UnityEngine.UI;

using CarRace.UI.Equipment;

namespace CarRace.UI
{
    public class UIElementsProvider : MonoBehaviour
    {
        [field: SerializeField] public Button EquipmentButton { get; private set; }
        [field: SerializeField] public EquipmentView EquipmentView { get; private set; }
    }
}