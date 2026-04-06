using System.Collections.Generic;
using UnityEngine;

namespace CarRace
{
    [CreateAssetMenu(fileName = "PartTracksConfig", menuName = "Tracks/Part Tracks Config")]
    public class PartTracksConfig : ScriptableObject
    {
        [SerializeField] private List<PartTrackData> _partTracksData;
        
        public IReadOnlyList<PartTrackData> PartTracksData => _partTracksData;
    }
}
