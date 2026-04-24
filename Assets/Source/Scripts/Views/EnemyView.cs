using System;
using Animancer;
using CarRace.Helpers;
using UnityEngine;

namespace CarRace
{
    public class EnemyView : MonoBehaviour, IPoolable<EnemyView>
    {
        [field: SerializeField] public AnimancerComponent Animancer { get; private set; }
        [field: SerializeField] public Transform Transform { get; private set; }

        public Action<EnemyView> Despawned { get; set; }
    }
}