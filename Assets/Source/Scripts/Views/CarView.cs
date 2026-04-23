using System.Collections.Generic;
using UnityEngine;

namespace CarRace.Views
{
    public class CarView : MonoBehaviour
    {
        [SerializeField] private List<Transform> _weaponsPlaces;
        
        IReadOnlyList<Transform>  WeaponsPlaces => _weaponsPlaces;
    }
}
