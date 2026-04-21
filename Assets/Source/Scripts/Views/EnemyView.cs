using Animancer;
using CarRace.Helpers;
using UnityEngine;

namespace CarRace
{
    public class EnemyView : MonoBehaviour, IPoolable
    {
        [field: SerializeField] public AnimancerComponent Animancer { get; private set; }
        
        public void SetContainer(Transform container)
        {
            
        }

        public void Despawn()
        {
        }
    }
}
