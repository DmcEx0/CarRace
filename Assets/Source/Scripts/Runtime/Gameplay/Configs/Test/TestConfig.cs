using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CarRace.Gameplay.Configs.Test
{
    [CreateAssetMenu(fileName = "TestConfig", menuName = "Configs/Test Config")]
    public class TestConfig : ScriptableObject
    {
        [field: SerializeField] public AssetReference CarReference { get; private set; }
        
        [SerializeField] private List<TestInitialWeaponData> _initialWeapons;
        
        public IReadOnlyList<TestInitialWeaponData> InitialWeapons => _initialWeapons;
    }
}