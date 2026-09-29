using UnityEngine;

namespace CarRace.Gameplay.Track
{
    public class PartTrack : MonoBehaviour
    {
        [field: SerializeField] public Transform EndPoint { get; private set; }
    }
}