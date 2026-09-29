using UnityEngine;
using VContainer;
using VContainer.Unity;

using CarRace.Gameplay.Configs.Test;

namespace CarRace.Composition.LifetimeScopes
{
    public class TestLifetimeScope : LifetimeScope
    {
        [SerializeField] private TestConfig _testConfig;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_testConfig);
        }
    }
}