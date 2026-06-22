using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

using CarRace.Composition.SceneContexts;
using CarRace.Gameplay.Car;
using CarRace.Gameplay.Configs;
using CarRace.Gameplay.Enemies;
using CarRace.Gameplay.Targeting;
using CarRace.Infrastructure.ObjectPooling;

namespace CarRace.Infrastructure.Factories
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