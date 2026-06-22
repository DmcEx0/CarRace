using UnityEngine;

using CarRace.Gameplay.Weapons;

namespace CarRace.Gameplay.Configs.Test
{
    [System.Serializable]
    public class TestInitialWeaponData
    {
        [field: SerializeField] public WeaponType Type { get; private set; }
        [field: SerializeField] public int Level { get; private set; }
    }
}
