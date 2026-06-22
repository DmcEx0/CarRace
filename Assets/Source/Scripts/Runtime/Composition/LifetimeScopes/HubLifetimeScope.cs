using UnityEngine;
using VContainer;
using VContainer.Unity;

using CarRace.Gameplay.Level;

namespace CarRace.Composition.LifetimeScopes
{
    public class HubLifetimeScope : LifetimeScope
    {
        [SerializeField] private LevelGate _levelGate;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_levelGate);
        }
    }
}
