using UnityEngine;

namespace CarRace.Helpers
{
    public interface IPoolable
    {
        public void SetContainer(Transform container);
        public void Despawn();
    }
}