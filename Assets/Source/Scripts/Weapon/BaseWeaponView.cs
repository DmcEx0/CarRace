using System.Collections.Generic;
using UnityEngine;

namespace CarRace
{
    public class BaseWeaponView : MonoBehaviour
    {
        [SerializeField] private Transform[] _firePoints;
        
        public IReadOnlyList<Transform> FirePoints => _firePoints;
    }
}
