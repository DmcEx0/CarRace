using System.Collections.Generic;
using System.Threading;
using CarRace.Factory;
using CarRace.Helpers;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CarRace
{
    public class ProjectilesFactory : GameObjectFactory
    {
        private const int MaxIterationsCount = 10;

        private readonly ObjectPool<BaseProjectileBehaviour> _pool;

        private BaseProjectileBehaviour _projectileBehaviour;

        private readonly List<AsyncOperationHandle> _operationHandles;

        public ProjectilesFactory(Transform container)
        {
            _pool = new ObjectPool<BaseProjectileBehaviour>(container);
            _operationHandles = new List<AsyncOperationHandle>();
        }

        public async UniTask PrepareAsync(WeaponContext weaponContext, int count, CancellationToken token)
        {
            for (int i = 0; i < count; i++)
            {
                if (i % MaxIterationsCount == 0)
                {
                    await UniTask.Yield();
                }

                var result = await CreateWithAddressAsync<ProjectileView>(weaponContext.ProjectileReference, token);

                BaseProjectileBehaviour projectileBehaviour = default;

                switch (weaponContext.Type)
                {
                    case WeaponType.Minigun:
                        projectileBehaviour =
                            new ForwardProjectileBehaviour(result.Instance, weaponContext.ProjectileSettings,
                                weaponContext.View.FirePoints[0]);
                        break;
                    case WeaponType.RocketLauncher:
                        projectileBehaviour =
                            new BallisticProjectileBehaviour(result.Instance, weaponContext.ProjectileSettings,
                                weaponContext.View.FirePoints[0]);
                        break;
                }

                _pool.AddInstance(projectileBehaviour);
                _operationHandles.Add(result.Handle);
            }
        }

        public BaseProjectileBehaviour Get(Vector3 targetPosition, Vector3 position)
        {
            var instance = _pool.Get();

            instance.Init(targetPosition);

            return instance;
        }

        public void ReleaseAll()
        {
            foreach (var operationHandle in _operationHandles)
            {
                Release(operationHandle);
            }

            _operationHandles.Clear();
        }
    }
}