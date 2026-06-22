using Unity.Cinemachine;
using UnityEngine;

namespace CarRace.Composition.SceneContexts
{
    public class BootstrapSceneContext : MonoBehaviour
    {
        [field: SerializeField] public CinemachineCamera Camera { get; private set; }
        [field: SerializeField] public Transform ProjectilePoolContainer { get; private set; }
    }
}