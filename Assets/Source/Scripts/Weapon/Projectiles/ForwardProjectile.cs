using UnityEngine;

namespace CarRace
{
    public class ForwardProjectile : BaseProjectile
    {
        public ForwardProjectile(ProjectileView view, float speed, Vector3 endPosition) : base(view, speed, endPosition) { }

        public override void OnMove(float deltaTime)
        {
            var rb = View.Rb;
            
            var direction = TargetPosition - rb.position;
            
            rb.MovePosition(rb.position + direction * (Speed * deltaTime));
        }
    }
}
