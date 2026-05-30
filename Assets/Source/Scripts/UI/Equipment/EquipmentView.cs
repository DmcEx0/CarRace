using UnityEngine;
using UnityEngine.UI;

namespace CarRace.UI
{
    public class EquipmentView : MonoBehaviour
    {
        [field: SerializeField] public RectTransform InventoryContainer { get; private set; }
        [field: SerializeField] public Button CloseButton { get; private set; }
    }
}