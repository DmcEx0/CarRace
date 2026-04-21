using Animancer;
using UnityEngine;

namespace CarRace
{
    public class EnemyView : MonoBehaviour
    {
        [field: SerializeField] public AnimancerComponent Animancer { get; private set; }
    }
}
