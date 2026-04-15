using UnityEngine;

namespace CarRace
{
    public abstract class BaseProjectile
    {
        public ProjectileView View {get; private set;}
        protected float Speed {get; private set;}
        protected Vector3 TargetPosition {get; private set;}

        protected BaseProjectile(ProjectileView view, float speed, Vector3 targetPosition)
        {
            View = view;
            Speed = speed;
            TargetPosition = targetPosition;
        }
        
        public abstract void OnMove(float deltaTime);
    }
}
