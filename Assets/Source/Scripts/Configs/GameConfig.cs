using UnityEngine;

namespace CarRace.Configs
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/Game Config")]
    public class GameConfig : ScriptableObject
    {
        [field: SerializeField] public LayerMask PlayerLayerMask {get; private set;}
        [field: SerializeField] public LayerMask EnemyLayerMask {get; private set;}
        [field: SerializeField] public LayerMask GroundLayerMask {get; private set;}
        [field: SerializeField] public LayerMask ObstacleLayerMask {get; private set;}
        
        [field: Space]
        [field: SerializeField] public int MaxTargetsCountForPlayer {get; private set;}
        
        [field: Space]
        [field: SerializeField] public int WeaponsNumberForMerge {get; private set;}
        
        [field: Space]
        [field: SerializeField, Min(0)] public int TargetFPS {get; private set;}
    }
}