using System;
using UnityEngine;

namespace CarRace.Helpers
{
    public interface IPoolable<T>
    {
        public Transform Transform { get; }
        public Action<T> Despawned { get; set; }
    }
}