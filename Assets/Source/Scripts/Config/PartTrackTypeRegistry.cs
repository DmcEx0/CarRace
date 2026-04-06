using System;
using System.Collections.Generic;
using UnityEngine;

namespace CarRace
{
    [CreateAssetMenu(fileName = "PartTrackTypeRegistry", menuName = "Tracks/Part Track Type Registry")]
    public class PartTrackTypeRegistry : ScriptableObject
    {
        [SerializeField] private List<string> _types = new();

        public IReadOnlyList<string> Types => _types;

        public bool Contains(string typeName)
        {
            if (string.IsNullOrWhiteSpace(typeName))
                return false;

            return _types.Contains(typeName);
        }

        public int IndexOf(string typeName)
        {
            if (string.IsNullOrWhiteSpace(typeName))
                return -1;

            return _types.IndexOf(typeName);
        }

        public bool TryAddType(string typeName)
        {
            if (string.IsNullOrWhiteSpace(typeName))
                return false;

            typeName = typeName.Trim();

            if (_types.Contains(typeName))
                return false;

            _types.Add(typeName);
            return true;
        }

        public string GetTypeName(int index)
        {
            if (index < 0 || index >= _types.Count)
                return string.Empty;

            return _types[index];
        }

        public string[] GetTypeNames()
        {
            return _types.ToArray();
        }
    }
}