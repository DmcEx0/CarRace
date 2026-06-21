using CarRace.Placeholders;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace CarRace.Scopes
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
