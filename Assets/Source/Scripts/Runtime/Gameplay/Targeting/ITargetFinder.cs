using UnityEngine;

namespace CarRace.Gameplay.Targeting
{
    public interface ITargetFinder<T>
    {
        public bool TryGetNearest(Vector3 position, float minDistance, out T target);
    }
}