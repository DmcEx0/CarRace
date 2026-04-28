using System.Collections.Generic;
using UnityEngine;

namespace CarRace.Configs
{
    [CreateAssetMenu(fileName = "TestConfig", menuName = "Configs/Test Config")]
    public class TestConfig : ScriptableObject
    {
        [SerializeField] private List<TestInitialWeaponData> _initialWeapons;
        
        public IReadOnlyList<TestInitialWeaponData> InitialWeapons => _initialWeapons;
    }
}