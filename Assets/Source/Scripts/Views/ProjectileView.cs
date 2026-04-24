using System;
using UnityEngine;

namespace CarRace
{
    public class ProjectileView : MonoBehaviour
    {
        [field: SerializeField] public Rigidbody Rb { get; private set; }
        [field: SerializeField] public Transform Transform { get; private set; }

        public Action<EnemyView> DetectedEnemy { get; set; }

        private void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent(out EnemyView enemyView))
            {
                DetectedEnemy?.Invoke(enemyView);

                return;
            }

            DetectedEnemy?.Invoke(null);
        }
    }
}