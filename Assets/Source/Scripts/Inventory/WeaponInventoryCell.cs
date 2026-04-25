namespace CarRace.Inventory
{
    public class WeaponInventoryCell
    {
        public int Level { get; private set; }
        public WeaponType Type { get; private set; }

        public WeaponInventoryCell(WeaponType type, int level)
        {
            Type = type;
            Level = level;
        }
    }
}