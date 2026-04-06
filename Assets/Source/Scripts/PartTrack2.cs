using UnityEngine;

namespace CarRace
{
    public class PartTrack2 : MonoBehaviour
    {
        [SerializeField] private PartTrackTypeRegistry _typeRegistry;

        [SerializeField] private string _typeName;

        [SerializeField] private int _includeMask;
        [SerializeField] private int _excludeMask;

        public PartTrackTypeRegistry TypeRegistry => _typeRegistry;
        public string TypeName => _typeName;
        public int IncludeMask => _includeMask;
        public int ExcludeMask => _excludeMask;

        public void SetRegistry(PartTrackTypeRegistry registry)
        {
            _typeRegistry = registry;
        }

        public void SetTypeName(string typeName)
        {
            _typeName = typeName;
        }

        public void SetIncludeMask(int mask)
        {
            _includeMask = mask;
        }

        public void SetExcludeMask(int mask)
        {
            _excludeMask = mask;
        }

        public bool CanConnectTo(PartTrack2 other)
        {
            if (_typeRegistry == null || other == null)
                return false;

            int otherTypeIndex = _typeRegistry.IndexOf(other.TypeName);
            if (otherTypeIndex < 0)
                return false;

            bool included = IsBitSet(_includeMask, otherTypeIndex);
            bool excluded = IsBitSet(_excludeMask, otherTypeIndex);

            if (_includeMask != 0 && included == false)
                return false;

            if (excluded)
                return false;

            return true;
        }

        private bool IsBitSet(int mask, int index)
        {
            if (index < 0 || index >= 32)
                return false;

            return (mask & (1 << index)) != 0;
        }
    }
}