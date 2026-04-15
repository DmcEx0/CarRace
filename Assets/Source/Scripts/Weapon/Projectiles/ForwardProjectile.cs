using UnityEngine;

namespace CarRace
{
    public class ForwardProjectile : BaseProjectile
    {
        public ForwardProjectile(ProjectileView view, float speed) : base(view, speed) { }

        public override void OnMove(Vector3 endPosition,  float deltaTime)
        {
            var rb = View.Rb;
            
            var direction = endPosition - rb.position;
            
            rb.MovePosition(rb.position + direction * Speed * deltaTime);
        }
    }
}
