using System.Collections.Generic;
using UnityEngine;

namespace CarRace.Gameplay.Weapons
{
    public class WeaponView : MonoBehaviour
    {
        [SerializeField] private Transform[] _firePoints;
        
        public IReadOnlyList<Transform> FirePoints => _firePoints;
    }
}