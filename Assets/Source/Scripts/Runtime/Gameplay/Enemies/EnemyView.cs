using System;
using Animancer;
using UnityEngine;
using UnityEngine.AI;

using CarRace.Infrastructure.ObjectPooling;

namespace CarRace.Gameplay.Enemies
{
    public class EnemyView : MonoBehaviour, IPoolable<EnemyView>
    {
        [field: SerializeField] public AnimancerComponent Animancer { get; private set; }
        [field: SerializeField] public Transform ViewTransform { get; private set; }
        [field: SerializeField] public NavMeshAgent Agent { get; private set; }

        public Action<EnemyView> Despawned { get; set; }
    }
}