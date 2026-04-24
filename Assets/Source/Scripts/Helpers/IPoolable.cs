using System;
using UnityEngine;

namespace CarRace.Helpers
{
    public interface IPoolable<T>
    {
        public Transform ViewTransform { get; }
        public Action<T> Despawned { get; set; }
    }
}