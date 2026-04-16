using UnityEngine;

namespace CarRace
{
    public class BallisticProjectile : BaseProjectile
    {
        private Vector3 _velocity;
        private float _gravity;

        public BallisticProjectile(ProjectileView view, float speed, Vector3 targetPosition)
            : base(view, speed, targetPosition)
        {
            _gravity = Physics.gravity.y;

            var start = view.Rb.position;
            var toTarget = targetPosition - start;

            var horizontal = new Vector3(toTarget.x, 0f, toTarget.z);
            float distance = horizontal.magnitude;

            float height = toTarget.y;

            float time = distance / speed;

            Vector3 horizontalVelocity = horizontal / time;

            float verticalVelocity = (height - 0.5f * _gravity * time * time) / time;

            _velocity = horizontalVelocity + Vector3.up * verticalVelocity;
        }

        public override void OnMove(float deltaTime)
        {
            var rb = View.Rb;

            _velocity.y += _gravity * deltaTime;

            rb.MovePosition(rb.position + _velocity * deltaTime);
        }
    }
}