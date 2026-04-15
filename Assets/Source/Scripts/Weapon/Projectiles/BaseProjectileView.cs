using UnityEngine;

namespace CarRace
{
    public class BaseProjectileView : MonoBehaviour
    {
        [field: SerializeField] public Rigidbody Rb { get; private set; }
    }
}
