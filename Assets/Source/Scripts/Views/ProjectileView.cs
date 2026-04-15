using System;
using UnityEngine;

namespace CarRace
{
    public class ProjectileView : MonoBehaviour
    {
        [field: SerializeField] public Rigidbody Rb { get; private set; }
        
        public Action<EnemyView> DetectedEnemy { get; set; }

        private void OnCollisionStay(Collision other)
        {
            if(other.collider.TryGetComponent(out EnemyView enemyView))
            {
                DetectedEnemy?.Invoke(enemyView);
            }
        }
    }
}
