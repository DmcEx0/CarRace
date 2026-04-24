using CarRace.Factory;
using UnityEngine;

namespace CarRace
{
    public class ProjectilesFactory : GameObjectFactory
    {
        private const int MaxIterationsCount = 10;

        private readonly ObjectPool<BaseProjectileBehaviour> _pool;

        private BaseProjectileBehaviour _projectileBehaviour;

        public ProjectilesFactory(Transform container)
        {
            _pool = new ObjectPool<BaseProjectileBehaviour>(container);
        }

        public async UniTask PrepareAsync(WeaponContext weaponContext, int count, CancellationToken token)
        {
            for (int i = 0; i < count; i++)
            {
                if (i % MaxIterationsCount == 0)
                {
                    await UniTask.Yield();
                }

                var instance = await CreateAsync<ProjectileView>(weaponContext.ProjectileReference, token);

                BaseProjectileBehaviour projectileBehaviour = default;

                switch (weaponContext.Type)
                {
                    case WeaponType.Minigun:
                        projectileBehaviour =
                            new ForwardProjectileBehaviour(instance, weaponContext.ProjectileSpeed);
                        break;
                    case WeaponType.RocketLauncher:
                        projectileBehaviour =
                            new BallisticProjectileBehaviour(instance, weaponContext.ProjectileSpeed);
                        break;
                }

                _pool.AddInstance(projectileBehaviour);
            }
        }

        public BaseProjectileBehaviour Get(Vector3 targetPosition, Vector3 position)
        {
            var instance = _pool.Get();

            instance.Init(targetPosition);
            instance.Transform.position = position;

            return instance;
        }
    }
}