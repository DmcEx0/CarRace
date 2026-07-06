using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CarRace.Gameplay.Weapons
{
    public class WeaponSlot
    {
        private readonly AsyncReactiveProperty<WeaponContext> _weaponContext;
        
        public Transform ParentTransform { get; private set; }
        public int Number { get; private set; }

        public IReadOnlyAsyncReactiveProperty<WeaponContext> WeaponContext => _weaponContext;

        public WeaponSlot(Transform parentTransform, int number)
        {
            ParentTransform = parentTransform;
            Number = number;
            
            _weaponContext = new AsyncReactiveProperty<WeaponContext>(null);
        }

        public void Replace(WeaponContext weaponContext)
        {
            Remove();

            _weaponContext.Value = weaponContext;
        }

        public void Remove()
        {
            if (_weaponContext.Value == null)
            {
                return;
            }
            
            _weaponContext.Value.ProjectilesFactory.Dispose();
            
            Object.Destroy(_weaponContext.Value.View.gameObject);
            
            _weaponContext.Value =  null;
        }
    }
}