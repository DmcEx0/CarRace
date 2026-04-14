using UnityEngine;

namespace CarRace.Helpers
{
    public class TargetSystem<T>
    {
        private readonly LayerMask _layerMask;
        private readonly Collider[] _findingTargets;
        
        // public T Target { get; private set; }

        public TargetSystem(LayerMask layerMask, int maxCount)
        {
            _layerMask = layerMask;
            _findingTargets = new Collider[maxCount];
        }

        public bool TryGetNearest(out T target, Vector3 position, float radius)
        {
            target = default;

            var count = Physics.OverlapSphereNonAlloc(position, radius, _findingTargets, _layerMask);

            if (count == 0)
            {
                return false;
            }

            Collider nearest = default;

            foreach (var t in _findingTargets)
            {
                if(t == null)
                {
                    continue; 
                }
                
                if(nearest == null)
                {
                    nearest = t;
                }
                
                var nearestDistance = (position - nearest.transform.position).sqrMagnitude;
                var tDistance = (position - t.transform.position).sqrMagnitude;
                
                if (tDistance <= nearestDistance)
                {
                    nearest = t;
                }
            }

            if (nearest == null || nearest.TryGetComponent(out target) == false)
            {
                target = default;
                return false;
            }

            return true;
        }
    }
}