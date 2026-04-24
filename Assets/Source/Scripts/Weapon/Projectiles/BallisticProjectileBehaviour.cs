using UnityEngine;

namespace CarRace
{
    public class BallisticProjectileBehaviour : BaseProjectileBehaviour
    {
        private Vector3 _velocity;
        private float _gravity;

        public BallisticProjectileBehaviour(ProjectileView view, float speed) : base(view, speed) { }

        public override void OnMove(float deltaTime)
        {
            var rb = View.Rb;

            _velocity.y += _gravity * deltaTime;

            rb.MovePosition(rb.position + _velocity * deltaTime);
        }
        
        protected override void OnInit()
        {
            _gravity = Physics.gravity.y;

            var start = View.Rb.position;
            var toTarget = TargetPosition - start;

            var horizontal = new Vector3(toTarget.x, 0f, toTarget.z);
            float distance = horizontal.magnitude;

            float height = toTarget.y;

            float time = distance / Speed;

            Vector3 horizontalVelocity = horizontal / time;

            float verticalVelocity = (height - 0.5f * _gravity * time * time) / time;

            _velocity = horizontalVelocity + Vector3.up * verticalVelocity;
        }
    }
}