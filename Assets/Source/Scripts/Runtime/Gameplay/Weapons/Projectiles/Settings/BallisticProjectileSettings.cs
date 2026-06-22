using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace CarRace.Gameplay.Weapons.Projectiles.Settings
{
    // Сохраняет [SerializeReference]-данные, сериализованные под старым неймспейсом CarRace.
    [MovedFrom(true, "CarRace", null, null)]
    [Serializable]
    public class BallisticProjectileSettings : ProjectileSettings
    {
        [field: SerializeField, Min(0f)] public float ArcHeight { get; private set; } = 2f;

        [field: SerializeField] public AnimationCurve HeightByProgress {get; private set;} =
            new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.5f, 1f), new Keyframe(1f, 0f));

        [field: SerializeField] public bool RotateAlongVelocity { get; private set; } = true;

        public Vector3 ArcDirection => Vector3.up;
    }
}