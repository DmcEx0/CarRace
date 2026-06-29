using UnityEngine;

namespace CarRace.Gameplay.Targeting
{
    public class SphereTargetFinder<T> : ITargetFinder<T>
    {
        private readonly LayerMask _layerMask;
        private readonly Collider[] _findingTargets;

        public SphereTargetFinder(LayerMask layerMask, int maxCount)
        {
            _layerMask = layerMask;
            _findingTargets = new Collider[maxCount];
        }

        public bool TryGetNearest(Vector3 position, float minDistance, out T target) //TODO: рефактор для update
        {
            var count = Physics.OverlapSphereNonAlloc(position, minDistance, _findingTargets, _layerMask);

            var hasTarget = false;
            var bestDistance = float.PositiveInfinity;
            T bestTarget = default;

            for (var i = 0; i < count; i++)
            {
                var collider = _findingTargets[i];

                if (collider == null || collider.TryGetComponent(out T candidate) == false)
                {
                    continue;
                }

                var distance = (position - collider.transform.position).sqrMagnitude;

                if (distance >= bestDistance)
                {
                    continue;
                }

                bestDistance = distance;
                bestTarget = candidate;
                hasTarget = true;
            }

            target = bestTarget;
            
            return hasTarget;
        }
    }
}