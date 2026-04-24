using System;
using UnityEngine;

namespace CarRace
{
    public class ProjectileView : MonoBehaviour
    {
        [field: SerializeField] public Rigidbody Rb { get; private set; }
        
        public Action<EnemyView> DetectedEnemy { get; set; }
        
        public Transform Transform { get; private set; }
        
        private void Start()
        {
            Transform = transform;
        }

        private void OnTriggerStay(Collider other)
        {
            if(other.TryGetComponent(out EnemyView enemyView))
            {
                DetectedEnemy?.Invoke(enemyView);
            }
            else
            {
                DetectedEnemy?.Invoke(null);
            }
        }
    }
}
