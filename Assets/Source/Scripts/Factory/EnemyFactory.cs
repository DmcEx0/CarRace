using System;
using System.Threading;
using CarRace.Configs;
using CarRace.Contexts;
using CarRace.Helpers;
using CarRace.Views;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CarRace.Factory
{
    public class EnemyFactory : GameObjectFactory, IDisposable
    {
        private readonly EnemiesConfig _config;
        private readonly ObjectPool<EnemyView> _pool;

        private SpawnResult<EnemyView> _spawnResult;

        public EnemyFactory(EnemiesConfig config, LevelSceneContext levelSceneContext)
        {
            _config = config;
            _pool = new ObjectPool<EnemyView>(levelSceneContext.EnemyPoolContainer);
        }

        public async UniTask PrepareAsync(int count, CancellationToken token)
        {
            _spawnResult = await CreateWithAddressAsync<EnemyView>(_config.Reference, token);

            for (int i = 0; i < count; i++)
            {
                var instance = Create(_spawnResult.Prefab);
                _pool.AddInstance(instance);
            }
        }

        public EnemyContext Get(Vector3 position)
        {
            var instance = _pool.Get();
            instance.transform.position = position;

            var context = new EnemyContext(instance, _config);

            return context;
        }

        public void Dispose()
        {
            _pool?.Dispose();
            _spawnResult.Release();
        }
    }
}