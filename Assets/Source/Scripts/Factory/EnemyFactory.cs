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
        private readonly EnemiesConfig _enemyConfig;
        private readonly GameConfig _gameConfig;
        private readonly ObjectPool<EnemyView> _pool;

        private SpawnResult<EnemyView> _spawnResult;

        public EnemyFactory(EnemiesConfig enemyConfig, GameConfig gameConfig, LevelSceneContext levelSceneContext)
        {
            _enemyConfig = enemyConfig;
            _gameConfig = gameConfig;
            
            _pool = new ObjectPool<EnemyView>(levelSceneContext.EnemyPoolContainer);
        }

        public async UniTask PrepareAsync(int count, CancellationToken token)
        {
            _spawnResult = await CreateWithAddressAsync<EnemyView>(_enemyConfig.Reference, token);

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
            
            instance.Agent.speed = _enemyConfig.Speed;
            instance.Agent.angularSpeed = _enemyConfig.AngularSpeed;

            var targetSystem = new TargetFinder<CarView>(_gameConfig.PlayerLayerMask, 1);
            var context = new EnemyContext(instance, _enemyConfig, targetSystem);

            return context;
        }

        public void Dispose()
        {
            _pool?.Dispose();
            _spawnResult.Release();
        }
    }
}