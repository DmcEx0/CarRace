using System.Collections.Generic;
using System.Linq;
using Alchemy.Inspector;
using CarRace.Gameplay.Configs;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CarRace.Gameplay.Track
{
    public class TrackBuilder : MonoBehaviour
    {
        [SerializeField] private PartTracksConfig _partsTracks;
        [SerializeField, Min(1)] private int _length;

        [SerializeField, Min(1)] private int _maxIterationForSpawnOne;

        [SerializeField] private bool _useSeed;
        [SerializeField, ShowIf("_useSeed")] private int _seed;

        [SerializeField] private bool _useAsync;

        private readonly List<PartTrack> _spawnedParts = new List<PartTrack>();

        private PartTrackData _lastPartTrackData;

        private void Start()
        {
            Build();
        }

        [Button]
        private void Build()
        {
            BuildAsync().Forget();
        }

        private async UniTask BuildAsync()
        {
            ResetTrack();

            if (_useSeed)
            {
                Random.InitState(_seed);
            }

            PartTrackData nextPartData = null;

            var dataToList = _partsTracks.PartTracksData.ToList();

            nextPartData = dataToList.Find(prt => prt.Type == PartTrackType.Forward);
            SpawnPart(nextPartData, Vector3.zero, Quaternion.identity);

            for (int i = 0; i < _length; i++)
            {
                nextPartData = null;

                bool foundConnect = false;
                int currentIteration = 0;

                while (foundConnect == false && currentIteration < _maxIterationForSpawnOne)
                {
                    currentIteration++;

                    nextPartData = _partsTracks.PartTracksData[Random.Range(0, _partsTracks.PartTracksData.Count)];

                    if (nextPartData.Type == PartTrackType.None)
                    {
                        continue;
                    }
                    
                    if(_lastPartTrackData.UseExclude == false && _lastPartTrackData.UseInclude == false)
                    {
                        foundConnect = true;
                    }

                    if (_lastPartTrackData.UseExclude)
                    {
                        if (_lastPartTrackData.Exclude.HasFlag(nextPartData.Type) == false)
                        {
                            foundConnect = true;
                        }
                    }

                    if (_lastPartTrackData.UseInclude)
                    {
                        if (_lastPartTrackData.Include.HasFlag(nextPartData.Type))
                        {
                            foundConnect = true;
                        }
                    }

                    if (_useAsync)
                    {
                        await UniTask.Yield();
                    }
                }

                if (foundConnect == false)
                {
                    continue;
                }

                var lastPart = _spawnedParts[^1];

                SpawnPart(nextPartData, lastPart.EndPoint.position, _spawnedParts[^1].EndPoint.rotation);
            }
        }

        private void SpawnPart(PartTrackData partTrack, Vector3 position, Quaternion rotation)
        {
            var nextPartInstance = Instantiate(partTrack.Prefab, transform, true);

            nextPartInstance.transform.position = position;
            nextPartInstance.transform.rotation = rotation;

            _spawnedParts.Add(nextPartInstance);

            _lastPartTrackData = partTrack;
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