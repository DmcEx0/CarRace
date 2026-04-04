using System;
using Alchemy.Inspector;
using UnityEngine;

namespace CarRace
{
    public class PartTrackData : MonoBehaviour
    {
        [field: SerializeField] public Transform EndPoint { get; private set; }
        
        [field: SerializeField] public PartTrackType Type { get; private set; }

        [field: SerializeField] public bool UseInclude { get; private set; }
        [field: SerializeField, ShowIf("UseInclude")]
        public PartTrackType Include { get; private set; }

        [field: SerializeField] public bool UseExclude { get; private set; }
        [field: SerializeField, ShowIf("UseExclude")]
        public PartTrackType Exclude { get; private set; }
    }
}