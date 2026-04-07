using System;
using Alchemy.Inspector;
using UnityEngine;

namespace CarRace
{
    [Serializable]
    public class PartTrackData
    {
        [SerializeField] private PartTrack _prefab;
        [SerializeField] private PartTrackType _type;

        [SerializeField] private bool _useInclude;
        [SerializeField, ShowIf("_useInclude")] private PartTrackType _include;

        [SerializeField] private bool _useExclude;
        [SerializeField, ShowIf("_useExclude")] private PartTrackType _exclude;

        public PartTrack Prefab => _prefab;
        public PartTrackType Type => _type;
        public bool UseInclude => _useInclude;
        public PartTrackType Include => _include;
        public bool UseExclude => _useExclude;
        public PartTrackType Exclude => _exclude;
        
        // [field: SerializeField] public PartTrack Prefab { get; private set; }
        //
        // [field: SerializeField] public PartTrackType Type { get; private set; }
        //
        // [field: SerializeField] public bool UseInclude { get; private set; }
        // [field: SerializeField, ShowIf("UseInclude")]
        // public PartTrackType Include { get; private set; }
        //
        // [field: SerializeField] public bool UseExclude { get; private set; }
        // [field: SerializeField, ShowIf("UseExclude")]
        // public PartTrackType Exclude { get; private set; }
    }
}