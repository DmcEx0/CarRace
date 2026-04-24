using System;
using UnityEngine;

namespace CarRace.Helpers
{
    public interface IPoolable
    {
        public Transform Transform { get; }
        public Action<IPoolable> Despawned { get; set; }
    }
}