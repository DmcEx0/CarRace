using System;
using Alchemy.Inspector;
using CarRace.Gameplay.Track;
using UnityEngine;

namespace CarRace.Gameplay.Configs
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
    }
}