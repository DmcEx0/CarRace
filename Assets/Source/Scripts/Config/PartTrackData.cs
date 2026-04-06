using System;
using Alchemy.Inspector;
using UnityEngine;

namespace CarRace
{
    [Serializable]
    public class PartTrackData
    {
        [SerializeField] private PartTrack _prefab;
        [SerializeField] private string _type;

        [SerializeField] private bool _useInclude;
        [SerializeField] private string _include;

        [SerializeField] private bool _useExclude;
        [SerializeField] private string _exclude;

        public PartTrack Prefab => _prefab;
        public string Type => _type;
        public bool UseInclude => _useInclude;
        public string Include => _include;
        public bool UseExclude => _useExclude;
        public string Exclude => _exclude;
        
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