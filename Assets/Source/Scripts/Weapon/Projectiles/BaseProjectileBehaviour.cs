using System;
using CarRace.Helpers;
using UnityEngine;

namespace CarRace
{
    public abstract class BaseProjectileBehaviour : IPoolable
    {
        private Transform _container;

        public ProjectileView View { get; private set; }
        protected float Speed { get; private set; }
        protected Vector3 TargetPosition { get; private set; }

        public Transform Transform => View.Transform;

        public Action<BaseProjectileBehaviour, EnemyView> DetectedEnemy { get; set; }

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

        private void OnViewEnemyDetected(EnemyView enemyView)
        {
            DetectedEnemy?.Invoke(this, enemyView);

            Despawn();
        }

        public abstract void OnMove(float deltaTime);

        public void SetContainer(Transform container)
        {
            _container = container;
        }

        public void Despawn()
        {
            View.DetectedEnemy -= OnViewEnemyDetected;

            Transform.SetActive(false);
            Transform.SetParent(_container);
        }

        protected virtual void OnInit()
        {
        }
    }
}