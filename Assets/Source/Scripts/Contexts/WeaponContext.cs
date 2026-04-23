using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CarRace
{
    public class WeaponContext
    {
        private readonly WeaponData _data;
        
        public WeaponView View { get; private set; }
        public AsyncOperationHandle OpHandle { get; private set; }

        public WeaponContext(WeaponData data, WeaponView view, AsyncOperationHandle opHandle)
        {
            _data = data;
            View = view;
            OpHandle = opHandle;
        }
    }
}
