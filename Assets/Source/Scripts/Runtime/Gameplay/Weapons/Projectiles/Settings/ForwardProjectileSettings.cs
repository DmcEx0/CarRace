using System;
using UnityEngine.Scripting.APIUpdating;

namespace CarRace.Gameplay.Weapons.Projectiles.Settings
{
    // Сохраняет [SerializeReference]-данные, сериализованные под старым неймспейсом CarRace.
    [MovedFrom(true, "CarRace", null, null)]
    [Serializable]
    public class ForwardProjectileSettings : ProjectileSettings
    {
    }
}
