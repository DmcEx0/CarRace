using UnityEngine;

namespace CarRace
{
    public class ForwardProjectileBehaviour : BaseProjectileBehaviour
    {
        public ForwardProjectileBehaviour(ProjectileView view, float speed) : base(view, speed) { }

        public override void OnMove(float deltaTime)
        {
            var rb = View.Rb;
            
            var direction = TargetPosition - rb.position;
            
            rb.MovePosition(rb.position + direction * (Speed * deltaTime));
        }
    }
}
