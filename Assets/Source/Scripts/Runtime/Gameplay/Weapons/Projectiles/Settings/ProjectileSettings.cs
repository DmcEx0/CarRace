using System;
using UnityEngine;

namespace CarRace.Gameplay.Weapons.Projectiles.Settings
{
    public abstract class ProjectileSettings
    {
        [field: SerializeField] public float Speed { get; private set; }
    }
}