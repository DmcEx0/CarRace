using Alchemy.Inspector;
using Animancer;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CarRace
{
    [CreateAssetMenu(fileName = "EnemiesConfig", menuName = "Configs/Enemies Config")]
    public class EnemiesConfig : ScriptableObject
    {
        [field: SerializeField] public EnemyView Prefab { get; private set; }
        [field: SerializeField] public AssetReference Reference { get; private set; }
        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public float FollowRadius { get; private set; }
        [field: SerializeField] public float AttackRadius { get; private set; }
        
        [field: SerializeField] public ClipTransition IdleAnimation { get; private set; }
        [field: SerializeField] public ClipTransition FollowAnimation { get; private set; }
    }
}