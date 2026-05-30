using CarRace.Views;
using UnityEngine;

namespace CarRace.Weapon
{
    public class ForwardProjectileBehaviour : BaseProjectileBehaviour
    {
        public ForwardProjectileBehaviour(ProjectileView view, ProjectileSettings settings, Transform firePoint) : base(view, settings, firePoint) { }

        public override void OnMove(float deltaTime)
        {
            if(View == null)
            {
                Debug.LogWarning("ForwardProjectileBehaviour.OnMove called without View");
                return;
            }
            
            var rb = View.Rb;
            
            var direction = TargetPosition - rb.position;
            
            rb.MovePosition(rb.position + direction * (Settings.Speed * deltaTime));
        }
    }
}
