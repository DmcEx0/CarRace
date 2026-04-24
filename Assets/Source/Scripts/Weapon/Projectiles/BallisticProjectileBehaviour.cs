using System;
using UnityEngine;

namespace CarRace
{
    public sealed class BallisticProjectileBehaviour : BaseProjectileBehaviour
    {
        private BallisticProjectileSettings _ballisticSettings;

        private Vector3 _startPosition;

        private float _currentTime;
        private float _duration;
        private float _distanceScale;

        public BallisticProjectileBehaviour(ProjectileView view, ProjectileSettings settings, Transform firePoint) :
            base(view, settings, firePoint)
        {
            if (Settings is BallisticProjectileSettings ballisticSettings)
            {
                _ballisticSettings = ballisticSettings;
            }
        }
        
        protected override void OnInit()
        {
            _startPosition = FirePoint.position;

            float distance = Vector3.Distance(_startPosition, TargetPosition);

            _distanceScale = Mathf.Clamp(distance * 0.1f, 1f, 10f);

            _currentTime = 0f;

            CalculateDuration();
        }

        public override void OnMove(float deltaTime)
        {
            if (_ballisticSettings == null)
                return;

            _currentTime += deltaTime;

            float progress = Mathf.Clamp01(_currentTime / _duration);

            if (progress >= 1f)
            {
                View.Rb.MovePosition(View.Rb.position + Vector3.down);
                return;
            }

            Vector3 position = Vector3.Lerp(_startPosition, TargetPosition, progress);

            float height = _ballisticSettings.HeightByProgress.Evaluate(progress);

            float arcHeight = height * _ballisticSettings.ArcHeight * _distanceScale;

            position.y += arcHeight;

            View.Rb.MovePosition(position);
        }

        private void CalculateDuration()
        {
            float distance = Vector3.Distance(FirePoint.position, TargetPosition);

            _duration = distance / Settings.Speed;

            _duration = Mathf.Max(0.01f, _duration);
        }
    }
}