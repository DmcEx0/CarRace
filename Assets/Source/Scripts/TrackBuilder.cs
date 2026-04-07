using System.Collections.Generic;
using System.Linq;
using Alchemy.Inspector;
using UnityEngine;

namespace CarRace
{
    public class TrackBuilder : MonoBehaviour
    {
        [SerializeField] private PartTracksConfig _partsTracks;
        [SerializeField] private int _length;
        
        [SerializeField] private int _maxIterationForSpawnOne;
        
        [SerializeField] private bool _useSeed;
        
        [SerializeField, ShowIf("_useSeed")] private int _seed;
        
        private readonly List<PartTrack> _spawnedParts = new List<PartTrack>();
        
        private PartTrackData _lastSpawnedPart;

        [Button]
        private void Build()
        {
            ResetTrack();

            if (_useSeed)
            {
                Random.InitState(_seed);
            }

            var dataToList = _partsTracks.PartTracksData.ToList();
            
            for (int i = 0; i < _length; i++)
            {
                PartTrackData nextPartData = null;
                
                if(i == 0)
                {
                    nextPartData =  dataToList.Find(prt => prt.Type == PartTrackType.Forward);
                    SpawnPart(nextPartData, Vector3.zero, Quaternion.identity);

                    continue;
                }

                bool cantConnect = false;
                int currentIteration = 0;
                
                
                while (cantConnect == false || _maxIterationForSpawnOne == currentIteration)
                {
                    currentIteration++;
                    
                    nextPartData = _partsTracks.PartTracksData[Random.Range(0, _partsTracks.PartTracksData.Count)];
                    
                    if(_lastSpawnedPart.UseExclude)
                    {
                        if(_lastSpawnedPart.Exclude.HasFlag(nextPartData.Type) == false)
                        {
                            cantConnect = true;
                        }
                    }
                    
                    if(_lastSpawnedPart.UseInclude)
                    {
                        if(_lastSpawnedPart.Include.HasFlag(nextPartData.Type) == false)
                        {
                            cantConnect = true;
                        }
                    }
                }
                
                SpawnPart(nextPartData, _lastSpawnedPart.Prefab.EndPoint.position, _spawnedParts[^1].EndPoint.rotation);
            }
        }
        
        private void SpawnPart(PartTrackData partTrack, Vector3 position, Quaternion rotation)
        {
            var nextPartInstance = Instantiate(partTrack.Prefab, transform, true);
            
            nextPartInstance.transform.position = position;
            nextPartInstance.transform.rotation = rotation;

            _spawnedParts.Add(nextPartInstance);

            _lastSpawnedPart = partTrack;
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