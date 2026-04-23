using System.Collections.Generic;

namespace CarRace.Inventory
{
    // Конкретная система, т.к. пока не понятно, будет ли инвентарь содержать что-то есть
    public class WeaponInventorySystem 
    {
        private readonly List<WeaponInventoryCell> _cells; // Может быть понадобится Collection.Observe
        
        public IReadOnlyList<WeaponInventoryCell> Cells => _cells;

        public WeaponInventorySystem()
        {
            _cells = new List<WeaponInventoryCell>();
        }
        
        public void Add(WeaponInventoryCell cell)
        {
            _cells.Add(cell);
        }
        
        public void Remove(WeaponInventoryCell cell)
        {
            _cells.Remove(cell);
        }
    }
}