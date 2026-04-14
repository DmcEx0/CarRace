using UnityEngine;

namespace CarRace.Factory
{
    public abstract class GameObjectFactory
    {
        protected T Create<T>(T prefab) where T : Object
        {
            var instance = Object.Instantiate(prefab);
            return instance;
        }
    }
}
