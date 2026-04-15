using UnityEngine;

namespace CarRace
{
    [CreateAssetMenu(fileName = "EnemiesConfig", menuName = "Configs/Enemies Config")]
    public class EnemiesConfig : ScriptableObject
    {
        [field: SerializeField] public int Count { get; private set; }
    }
}
