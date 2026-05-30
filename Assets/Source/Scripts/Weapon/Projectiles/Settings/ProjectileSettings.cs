using System;
using UnityEngine;

namespace CarRace.Weapon
{
    public abstract class ProjectileSettings
    {
        [field: SerializeField] public float Speed { get; private set; }
    }
}