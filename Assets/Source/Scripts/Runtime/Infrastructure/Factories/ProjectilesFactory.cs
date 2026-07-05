using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

using CarRace.Composition.SceneContexts;
using CarRace.Gameplay.Weapons;
using CarRace.Gameplay.Weapons.Projectiles;
using CarRace.Infrastructure.ObjectPooling;

namespace CarRace.Infrastructure.Factories
{
    public class ProjectilesFactory : GameObjectFactory, IDisposable
    {
        private readonly ObjectPool<BaseProjectileBehaviour> _pool;

        private BaseProjectileBehaviour _projectileBehaviour;

        private SpawnResult<ProjectileView> _spawnResult;

        public ProjectilesFactory(BootstrapSceneContext bootstrapSceneContext)
        {
            _pool = new ObjectPool<BaseProjectileBehaviour>(bootstrapSceneContext.ProjectilePoolContainer);
        }

        public async UniTask PrepareAsync(WeaponContext weaponContext, int count, CancellationToken token)
        {
            _spawnResult = await CreateWithAddressAsync<ProjectileView>(weaponContext.ProjectileReference, token);

            for (int i = 0; i < count; i++)
            {
                BaseProjectileBehaviour projectileBehaviour = default;

                var instance = Create(_spawnResult.Prefab);
                
                switch (weaponContext.Type)
                {
                    case WeaponType.Minigun:
                        projectileBehaviour =
                            new ForwardProjectileBehaviour(instance, weaponContext.ProjectileSettings,
                                weaponContext.View.FirePoints[0]);
                        break;
                    case WeaponType.RocketLauncher:
                        projectileBehaviour =
                            new BallisticProjectileBehaviour(instance, weaponContext.ProjectileSettings,
                                weaponContext.View.FirePoints[0]);
                        break;
                }

                _pool.AddInstance(projectileBehaviour);
            }
        }

        public BaseProjectileBehaviour Get(Vector3 targetPosition, Vector3 position)
        {
            var instance = _pool.Get();
            instance.Init(targetPosition);

            return instance;
        }

        public void Dispose()
        {
            _pool?.Dispose();
            _spawnResult.Release();
        }
    }
}