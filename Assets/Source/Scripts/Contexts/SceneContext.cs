using Unity.Cinemachine;
using UnityEngine;

namespace CarRace.Contexts
{
    public class SceneContext : MonoBehaviour
    {
        [field: SerializeField] public CinemachineCamera Camera { get; private set; }
        [field: SerializeField] public Transform ProjectilePoolContainer { get; private set; }
    }
}