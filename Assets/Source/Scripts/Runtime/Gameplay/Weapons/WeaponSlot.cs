using Cysharp.Threading.Tasks;
using UnityEngine;

using CarRace.Infrastructure.Factories;

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
            if (WeaponContext.Value == null)
            {
                return;
            }
            
            _weaponContext.Value.ProjectilesFactory.Dispose();
            _weaponContext.Value.SpawnResult.Release();
            
            Object.Destroy(_weaponContext.Value.View);
            
            _weaponContext.Value =  null;
        }
    }
}