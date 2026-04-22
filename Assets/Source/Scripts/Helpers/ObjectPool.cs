using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CarRace.Helpers
{
    public class ObjectPool<T> where T : MonoBehaviour, IPoolable
    {
        private readonly Queue<T> _pool = new();

        private Transform _container;

        public ObjectPool(Transform container)
        {
            _container = container;
        }

        public void Create(T prefab, int count)
        {
            if (_container == null)
            {
                CreateContainer();
            }

            for (int i = 0; i < count; i++)
            {
                var instance = Object.Instantiate(prefab, _container);
                
                _pool.Enqueue(instance);

                prefab.gameObject.SetActive(false);
                prefab.transform.SetParent(_container);
                prefab.SetContainer(_container);
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