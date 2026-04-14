using System;
using UnityEngine;
using UnityEngine.UI;

namespace CarRace
{
    [Serializable]
    public class Weapon
    {
        public int Level;
        public GameObject WeaponPrefab;
        public Sprite WeaponPreview;
    }
}
