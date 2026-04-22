using System.Runtime.Serialization;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace CarRace
{
    public class GarageLifeTimeScope : LifetimeScope
    {
        [SerializeField] private OwnedWeapons _ownedWeapons;
        [SerializeField] private CanvasView _canvasView;
        [SerializeField] private GarageCar _car;
        [SerializeField] private WeaponsConfig _weapons;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_weapons);
            builder.RegisterComponent(_ownedWeapons);
            builder.RegisterComponent(_canvasView);
            builder.RegisterComponent(_car);
            builder.RegisterEntryPoint<CanvasController>();
        }
    }
}



