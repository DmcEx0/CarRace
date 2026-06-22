using UnityEngine;
using VContainer;
using VContainer.Unity;

using CarRace.Dev.Prototypes;
using CarRace.Gameplay.Configs;

namespace CarRace.Composition.LifetimeScopes
{
    public class GarageLifeTimeScope : LifetimeScope
    {
        [SerializeField] private CanvasView _canvasView;
        [SerializeField] private GarageCar _car;
        [SerializeField] private WeaponsConfig _weapons;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_weapons);
            builder.RegisterComponent(_canvasView);
            builder.RegisterComponent(_car);
            
            builder.RegisterEntryPoint<CanvasController>();
        }
    }
}

