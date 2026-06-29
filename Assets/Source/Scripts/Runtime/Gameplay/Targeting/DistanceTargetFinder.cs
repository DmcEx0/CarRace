using UnityEngine;

namespace CarRace.Gameplay.Targeting
{
    public class DistanceTargetFinder<T> : ITargetFinder<T>
    {
        private readonly Transform _target;

        public DistanceTargetFinder(Transform target)
        {
            _target = target;
        }

        public bool TryGetNearest(Vector3 position, float minDistance, out T target)
        {
            var distance = (_target.position - position).sqrMagnitude;

            if (distance < minDistance)
            {
                target = _target.GetComponent<T>();
                return true;
            }
            
            target = default;
            return false;
        }
    }
}