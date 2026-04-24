using System;
using UnityEngine;

namespace CarRace
{
    public sealed class BallisticProjectileBehaviour : BaseProjectileBehaviour
    {
        private AnimationCurve _heightByProgress;
        private float _arcHeight;
        private bool _rotateAlongVelocity;
        private Vector3 _arcDirection;
        private Action<EnemyView> _baseEnemyDetectedHandler;

        private float _curveStartValue;
        private float _curveEndValue;
        private float _elapsed;
        private float _duration;
        private bool _isFinished;
        private Vector3 _startPosition;
        private Vector3 _previousPosition;

        public BallisticProjectileBehaviour(ProjectileView view, ProjectileSettings settings, Transform firePoint) :
            base(view, settings, firePoint)
        { }

        protected override void OnInit()
        {
            if (Settings is BallisticProjectileSettings ballisticSettings)
            {
                _arcHeight = Mathf.Max(0f, ballisticSettings.ArcHeight);
                _heightByProgress = CopyCurve(ballisticSettings.HeightByProgress);
                _rotateAlongVelocity = ballisticSettings.RotateAlongVelocity;
                _arcDirection = ballisticSettings.ArcDirection.sqrMagnitude > 0f
                    ? ballisticSettings.ArcDirection.normalized
                    : Vector3.up;
            }
            
            _elapsed = 0f;
            _isFinished = false;
            _startPosition = FirePoint.position;
            _previousPosition = _startPosition;
            _duration = CalculateDuration(_startPosition, TargetPosition);
            _curveStartValue = _heightByProgress.Evaluate(0f);
            _curveEndValue = _heightByProgress.Evaluate(1f);
        }

        public override void OnMove(float deltaTime)
        {
            if (_isFinished)
            {
                return;
            }

            if (_duration <= 0f)
            {
                FinishFlight();
                return;
            }

            _elapsed = Mathf.Min(_elapsed + deltaTime, _duration);

            float progress = Mathf.Clamp01(_elapsed / _duration);
            Vector3 linearPosition = Vector3.Lerp(_startPosition, TargetPosition, progress);
            float curveValue = _heightByProgress.Evaluate(progress);
            float normalizedCurveValue = curveValue - Mathf.Lerp(_curveStartValue, _curveEndValue, progress);
            Vector3 nextPosition = linearPosition + _arcDirection * (normalizedCurveValue * _arcHeight);
            Vector3 movement = nextPosition - _previousPosition;

            ViewTransform.position = nextPosition;

            if (_rotateAlongVelocity && movement.sqrMagnitude > 0.000001f)
            {
                ViewTransform.forward = movement.normalized;
            }

            _previousPosition = nextPosition;

            if (progress >= 1f)
            {
                FinishFlight();
            }
        }

        private float CalculateDuration(Vector3 startPosition, Vector3 targetPosition)
        {
            if (Settings.Speed <= 0f)
            {
                return 0f;
            }

            return Vector3.Distance(startPosition, targetPosition) / Settings.Speed;
        }

        private void FinishFlight()
        {
            _isFinished = true;
            ViewTransform.position = TargetPosition;

            if (_baseEnemyDetectedHandler != null)
            {
                // Base class subscribes on Init, so we mirror the unsubscribe on non-hit despawn.
                View.DetectedEnemy -= _baseEnemyDetectedHandler;
            }

            Despawned?.Invoke(this);
        }

        private static AnimationCurve CopyCurve(AnimationCurve sourceCurve)
        {
            if (sourceCurve == null || sourceCurve.length == 0)
            {
                return new AnimationCurve(
                    new Keyframe(0f, 0f),
                    new Keyframe(0.5f, 1f),
                    new Keyframe(1f, 0f));
            }

            return new AnimationCurve(sourceCurve.keys);
        }
    }
}