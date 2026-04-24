using System;
using System.Collections.Generic;
using UnityEngine;
 
namespace CarRace.Helpers
{
    public class ObjectPool<T> where T : IPoolable
    {
        private readonly Queue<T> _pool = new();

        private Transform _container;

        public ObjectPool(Transform container)
        {
            _container = container;
        }

        public void AddInstance(T instance)
        {
            if (_container == null)
            {
                CreateContainer();
            }

            _pool.Enqueue(instance);

            instance.Despawned += OnDespawned;
            
            instance.Transform.SetActive(false);
            instance.Transform.SetParent(_container);
        }

        private void OnDespawned(T instance)
        {
            _pool.Enqueue(instance);
        }

        public T Get() //TODO: добавить авто-расширение пула
        {
            if (_pool.Count != 0)
            {
                var instance = _pool.Dequeue();
                instance.Transform.SetActive(true);
                instance.Transform.SetParent(null);

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