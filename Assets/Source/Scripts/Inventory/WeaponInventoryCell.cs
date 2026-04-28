using System;

namespace CarRace.Inventory
{
    public struct WeaponInventoryCell : IEquatable<WeaponInventoryCell>
    {
        public int Level { get; private set; }
        public WeaponType Type { get; private set; }

        public WeaponInventoryCell(WeaponType type, int level)
        {
            Type = type;
            Level = level;
        }

        public bool Equals(WeaponInventoryCell other)
        {
            return Level == other.Level && Type == other.Type;
        }

        public override bool Equals(object obj)
        {
            return obj is WeaponInventoryCell other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Level, (int)Type);
        }
    }
}