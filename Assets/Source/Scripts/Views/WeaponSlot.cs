using CarRace.Factory;
using UnityEngine;

namespace CarRace.Views
{
    public class WeaponSlot
    {
        public Transform ParentTransform { get; private set; }
        public int Number { get; private set; }
        public WeaponContext WeaponContext { get; private set; }

        public WeaponSlot(Transform parentTransform, int number)
        {
            ParentTransform = parentTransform;
            Number = number;
        }

        public void Replace(WeaponContext weaponContext)
        {
            Remove();

            WeaponContext = weaponContext;
        }

        public void Remove()
        {
            if (WeaponContext == null)
            {
                return;
            }

            GameObjectFactory.Release(WeaponContext.OpHandle);
            WeaponContext =  null;
        }
    }
}