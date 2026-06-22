using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CarRace.UI.Equipment
{
    public class EquipmentCellView : MonoBehaviour
    {
        [field: SerializeField] public Image Image { get; private set; }
        [field: SerializeField] public TMP_Text Text { get; private set; }
    }
}