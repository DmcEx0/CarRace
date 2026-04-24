using System;
using CarRace.Helpers;
using UnityEngine;

namespace CarRace
{
    public abstract class BaseProjectileBehaviour : IPoolable<BaseProjectileBehaviour>
    {
        public ProjectileView View { get; private set; }
        protected Vector3 TargetPosition { get; private set; }
        protected Transform FirePoint { get; private set; }
        protected ProjectileSettings Settings { get; private set; }

        public Transform ViewTransform => View.Transform;

        public Action<BaseProjectileBehaviour, EnemyView> DetectedEnemy { get; set; }
        
        public Action<BaseProjectileBehaviour> Despawned { get; set; }

        protected BaseProjectileBehaviour(ProjectileView view, ProjectileSettings settings, Transform firePoint)
        {
            View = view;
            Settings = settings;
            FirePoint = firePoint;
        }

        public void Init(Vector3 targetPosition)
        {
            ViewTransform.position = FirePoint.position;
            
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
            Despawned?.Invoke(this);
            DetectedEnemy?.Invoke(this, enemyView);
            View.DetectedEnemy -= OnViewEnemyDetected;
        }
    }
}