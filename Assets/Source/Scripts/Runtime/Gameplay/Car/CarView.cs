using System.Collections.Generic;
using UnityEngine;

namespace CarRace.Gameplay.Car
{
    public class CarView : MonoBehaviour
    {
        [SerializeField] private List<Transform> _weaponsSlotsTransform;

        public IReadOnlyList<Transform> WeaponsSlotsTransform => _weaponsSlotsTransform;
    }
}