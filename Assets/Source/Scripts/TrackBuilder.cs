using System.Collections.Generic;
using Alchemy.Inspector;
using UnityEngine;

namespace CarRace
{
    public class TrackBuilder : MonoBehaviour
    {
        [SerializeField] private List<PartTrackData> _partsTrackData;
        [SerializeField] private int _length;

        private List<PartTrack> _spawnedParts = new List<PartTrack>();

        [SerializeField] private bool _useSeed;
        [SerializeField, ShowIf("_useSeed")] private int _seed;

        [Button]
        private void Build()
        {
            ResetTrack();

            if (_useSeed)
            {
                Random.InitState(_seed);
            }

            for (int i = 0; i < _length; i++)
            {
                int nextPartIndex = -1;
                
                if(i == 0)
                {
                    nextPartIndex = _partsTrackData.FindIndex(prt => prt.Type == PartTrackType.Forward);
                    SpawnPart(nextPartIndex, Vector3.zero, Quaternion.identity);
                    
                    continue;
                }

                nextPartIndex = Random.Range(0, _partsTrackData.Count);
                
                SpawnPart(nextPartIndex, _spawnedParts[^1].EndPoint.position, _spawnedParts[^1].EndPoint.rotation);
            }
        }
        
        private void SpawnPart(int index, Vector3 position, Quaternion rotation)
        {
            var nextPartInstance = Instantiate(_partsTrackData[index].PartTrack, transform, true);
            
            nextPartInstance.transform.position = position;
            nextPartInstance.transform.rotation = rotation;

            _spawnedParts.Add(nextPartInstance);
        }

        [Button]
        private void ResetTrack()
        {
            if (_spawnedParts.Count == 0)
            {
                return;
            }

            foreach (var part in _spawnedParts)
            {
                DestroyImmediate(part.gameObject);
            }

            _spawnedParts.Clear();
        }
    }
}