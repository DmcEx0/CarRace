using UnityEngine;

namespace CarRace
{
    public class ForwardProjectileBehaviour : BaseProjectileBehaviour
    {
        public ForwardProjectileBehaviour(ProjectileView view, ProjectileSettings settings, Transform firePoint) : base(view, settings, firePoint) { }

        public override void OnMove(float deltaTime)
        {
            var rb = View.Rb;
            
            var direction = TargetPosition - rb.position;
            
            rb.MovePosition(rb.position + direction * (Settings.Speed * deltaTime));
        }
    }
}
