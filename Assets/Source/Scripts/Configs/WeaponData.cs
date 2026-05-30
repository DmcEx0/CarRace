using System;
using CarRace.Views;
using CarRace.Weapon;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CarRace.Configs
{
    [Serializable]
    public class WeaponData
    {
        [field: SerializeField] public Sprite WeaponPreview { get; private set; }
        [field: SerializeField] public WeaponView WeaponViewPrefab { get; private set; }
        [field: SerializeField] public AssetReference WeaponReference { get; private set; }
        [field: SerializeField] public ProjectileView ProjectileViewPrefab { get; private set; }
        [field: SerializeField] public AssetReference ProjectileReference { get; private set; }
        
        [field: SerializeReference] public ProjectileSettings ProjectileSettings { get; private set; }
        
        [field: Space]
        [field: SerializeField] public WeaponType Type { get; private set; }
        
        [field: Space]
        [field: SerializeField] public int Level { get; private set; }
        
        [field: Space]
        [field: SerializeField] public float BaseDamage { get; private set; }
        [field: SerializeField] public float BaseFireRate { get; private set; }
        [field: SerializeField] public float BaseRange { get; private set; }
    }
}