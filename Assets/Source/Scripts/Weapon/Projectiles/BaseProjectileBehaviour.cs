using System;
using CarRace.Helpers;
using UnityEngine;

namespace CarRace
{
    public abstract class BaseProjectileBehaviour : IPoolable
    {
        public ProjectileView View { get; private set; }
        protected float Speed { get; private set; }
        protected Vector3 TargetPosition { get; private set; }

        public Transform Transform => View.Transform;

        public Action<BaseProjectileBehaviour, EnemyView> DetectedEnemy { get; set; }
        
        public Action<IPoolable> Despawned { get; set; }

        protected BaseProjectileBehaviour(ProjectileView view, float speed)
        {
            View = view;
            Speed = speed;
        }

        public void Init(Vector3 targetPosition)
        {
            TargetPosition = targetPosition;
            View.DetectedEnemy += OnViewEnemyDetected;

            OnInit();
        }

        public abstract void OnMove(float deltaTime);

        protected virtual void OnInit()
        {
        }
        
        private void OnViewEnemyDetected(EnemyView enemyView)
        {
            DetectedEnemy?.Invoke(this, enemyView);
            View.DetectedEnemy -= OnViewEnemyDetected;
        }
    }
}