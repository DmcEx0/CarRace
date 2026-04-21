using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace CarRace.Helpers
{
    public class ObjectPool<T> where T : MonoBehaviour, IPoolable
    {
        private readonly Queue<T> _pool = new();

        private T _firstInstance;

        private Transform _container;

        public ObjectPool(Transform container)
        {
            _container = container;
        }

        public void Create(IEnumerable<T> instances)
        {
            if (_container == null)
            {
                CreateContainer();
            }

            foreach (var instance in instances)
            {
                if (_firstInstance == false)
                {
                    _firstInstance = instance;
                }

                _pool.Enqueue(instance);

                instance.gameObject.SetActive(false);
                instance.transform.SetParent(_container);
                instance.SetContainer(_container);
            }
        }

        public T Get() //TODO: добавить авто-расширение пула
        {
            if (_pool.Count != 0)
            {
                var instance = _pool.Dequeue();
                instance.gameObject.SetActive(true);
                instance.transform.SetParent(null);

                return instance;
            }

            throw new Exception($"Pool for {typeof(T).Name} is empty");
        }

        private void CreateContainer()
        {
            var parent = new GameObject();
            parent.name = $"Container_{typeof(T).Name}";
            _container = parent.transform;
        }
    }
}