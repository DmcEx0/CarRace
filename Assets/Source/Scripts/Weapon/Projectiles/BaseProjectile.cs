using UnityEngine;

namespace CarRace
{
    public abstract class BaseProjectile
    {
        protected ProjectileView View {get; private set;}
        protected float Speed {get; private set;}

        public BaseProjectile(ProjectileView view, float speed)
        {
            View = view;
            Speed = speed;
        }
        
        public abstract void OnMove(Vector3 endPosition, float deltaTime);
    }
}
