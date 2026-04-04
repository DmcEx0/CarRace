using System.Collections.Generic;
using Alchemy.Inspector;
using UnityEngine;

namespace CarRace
{
    public class TrackBuilder : MonoBehaviour
    {
        [SerializeField] private List<PartTrackData> _partsTrackData;
        [SerializeField] private int _length;
        
        [SerializeField] private int _maxIterationForSpawnOne;
        
        [SerializeField] private bool _useSeed;
        
        [SerializeField, ShowIf("_useSeed")] private int _seed;
        
        private readonly List<PartTrackData> _spawnedParts = new List<PartTrackData>();

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
                PartTrackData nextPart = null;
                
                if(i == 0)
                {
                    nextPart = _partsTrackData.Find(prt => prt.Type == PartTrackType.Forward);
                    SpawnPart(nextPart, Vector3.zero, Quaternion.identity);
                    
                    continue;
                }

                bool cantConnect = false;
                int currentIteration = 0;
                
                var previousPart = _spawnedParts[^1];
                
                while (cantConnect == false || _maxIterationForSpawnOne == currentIteration)
                {
                    currentIteration++;
                    
                    nextPart = _partsTrackData[Random.Range(0, _partsTrackData.Count)];
                    
                    if(previousPart.UseExclude)
                    {
                        if(previousPart.Exclude.HasFlag(nextPart.Type) == false)
                        {
                            cantConnect = true;
                        }
                    }
                    
                    if(previousPart.UseInclude)
                    {
                        if(previousPart.Include.HasFlag(nextPart.Type) == false)
                        {
                            cantConnect = true;
                        }
                    }
                }
                
                SpawnPart(nextPart, _spawnedParts[^1].EndPoint.position, _spawnedParts[^1].EndPoint.rotation);
            }
        }
        
        private void SpawnPart(PartTrackData partTrack, Vector3 position, Quaternion rotation)
        {
            var nextPartInstance = Instantiate(partTrack, transform, true);
            
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