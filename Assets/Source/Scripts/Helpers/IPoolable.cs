using UnityEngine;

namespace CarRace.Helpers
{
    public interface IPoolable
    {
        public Transform Transform { get; }
        public void SetContainer(Transform container);
        public void Despawn();
    }
}