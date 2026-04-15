using System.Collections.Generic;
using UnityEngine;

namespace CarRace
{
    public class WeaponView : MonoBehaviour
    {
        [SerializeField] private Transform[] _firePoints;
        
        public IReadOnlyList<Transform> FirePoints => _firePoints;
    }
}