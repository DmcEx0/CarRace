using CarRace.UI.Equipnet;
using UnityEngine;
using UnityEngine.UI;

namespace CarRace
{
    public class UIElementsProvider : MonoBehaviour
    {
        [field: SerializeField] public Button EquipmentButton { get; private set; }
        [field: SerializeField] public EquipmentView EquipmentView { get; private set; }
    }
}