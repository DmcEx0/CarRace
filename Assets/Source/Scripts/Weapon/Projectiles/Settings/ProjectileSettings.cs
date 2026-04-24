using System;
using UnityEngine;

namespace CarRace
{
    public abstract class ProjectileSettings
    {
        [field: SerializeField] public float Speed { get; private set; }
    }
}