using Animancer;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CarRace.Gameplay.Configs
{
    [CreateAssetMenu(fileName = "EnemiesConfig", menuName = "Configs/Enemies Config")]
    public class EnemiesConfig : ScriptableObject
    {
        [field: SerializeField] public AssetReference Reference { get; private set; }
        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public float AngularSpeed { get; private set; }
        [field: SerializeField] public float FollowRadius { get; private set; }
        [field: SerializeField] public float AttackRadius { get; private set; }
        
        [field: SerializeField] public ClipTransition IdleAnimation { get; private set; }
        [field: SerializeField] public ClipTransition FollowAnimation { get; private set; }
    }
}