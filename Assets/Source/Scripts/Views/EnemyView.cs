using Animancer;
using CarRace.Helpers;
using UnityEngine;

namespace CarRace
{
    public class EnemyView : MonoBehaviour, IPoolable
    {
        [field: SerializeField] public AnimancerComponent Animancer { get; private set; }

        private Transform _container;

        public Transform Transform { get; private set; }

        private void Start()
        {
            Transform = transform;
        }

        public void SetContainer(Transform container)
        {
            _container = container;
        }

        public void Despawn()
        {
            gameObject.SetActive(false);
            Transform.SetParent(_container);
        }
    }
}