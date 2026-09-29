using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CarRace.Gameplay.Configs
{
    [CreateAssetMenu(fileName = "PartTracksConfig", menuName = "Tracks/Part Tracks Config")]
    public class PartTracksConfig : ScriptableObject
    {
        [SerializeField] private List<PartTrackData> _partTracksData = new();

        public IReadOnlyList<PartTrackData> PartTracksData => _partTracksData;
    }
}
