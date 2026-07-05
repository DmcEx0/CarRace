using UnityEngine;

namespace CarRace.Gameplay.Targeting
{
    public class DistanceTargetFinder<T> : ITargetFinder<T> where T : Component
    {
        private readonly T _target;

        public DistanceTargetFinder(T target)
        {
            _target = target;
        }

        public bool TryGetNearest(Vector3 position, float minDistance, out T target)
        {
            var distance = (_target.transform.position - position).sqrMagnitude;
            target = _target;

            if (distance < minDistance)
            {
                return true;
            }

            return false;
        }
    }
}