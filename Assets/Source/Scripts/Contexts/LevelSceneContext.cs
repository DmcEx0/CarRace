using UnityEngine;

namespace CarRace.Contexts
{
    public class LevelSceneContext : MonoBehaviour
    {
        [field: SerializeField] public Transform EnemySpawnPointsContainer { get; private set; }
        [field: SerializeField] public Transform EnemyPoolContainer { get; private set; }
    }
}