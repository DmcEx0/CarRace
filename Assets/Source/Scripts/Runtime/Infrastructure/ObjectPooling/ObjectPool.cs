using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CarRace.Infrastructure.ObjectPooling
{
    public class ObjectPool<T> : IDisposable where T : class, IPoolable<T>
    {
        private readonly Queue<T> _pool;
        private readonly Queue<T> _freeInstances;

        private Transform _container;

        public ObjectPool(Transform container)
        {
            _pool = new Queue<T>();
            _freeInstances = new Queue<T>();

            _container = container;
        }

        public void Dispose()
        {
            ClearContainer(_pool);
            ClearContainer(_freeInstances);
        }

        public void AddInstance(T instance)
        {
            if (_container == null) //TODO: обязательно передавать контейнер
            {
                CreateContainer();
            }

            instance.Despawned += OnDespawned;

            ConfigureInstance(instance);
        }

        private void OnDespawned(T instance)
        {
            _freeInstances.Dequeue();

            ConfigureInstance(instance);
        }

        public T Get() //TODO: добавить авто-расширение пула
        {
            if (_pool.Count != 0)
            {
                var instance = _pool.Dequeue();

                instance.ViewTransform.SetActive(true);
                instance.ViewTransform.SetParent(null);

                _freeInstances.Enqueue(instance);

                return instance;
            }

            throw new Exception($"Pool for {typeof(T).Name} is empty");
        }

        private void ConfigureInstance(T instance)
        {
            instance.ViewTransform.SetActive(false);
            instance.ViewTransform.SetParent(_container);

            _pool.Enqueue(instance);
        }

        private void CreateContainer()
        {
            var parent = new GameObject();
            parent.name = $"Container_{typeof(T).Name}";
            _container = parent.transform;
        }
        
        private void ClearContainer(Queue<T> queue)
        {
            foreach (var instance in queue)
            {
                instance.Despawned -= OnDespawned;
                Object.Destroy(instance.ViewTransform.gameObject);
            }

            queue.Clear();
        }
    }
}