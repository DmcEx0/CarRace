using System.Collections.Generic;
using UnityEngine;

namespace CarRace
{
    [CreateAssetMenu(fileName = "OwnedWeapons", menuName = "Scriptable Objects/OwnedWeapons")]
    public class OwnedWeapons : ScriptableObject
    {
        public List<Weapon> Weapons;
    }
}
