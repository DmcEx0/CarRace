using System;
using System.Collections.Generic;
using System.Threading;
using CarRace.Contexts;
using CarRace.Factory;
using CarRace.FSM;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;

namespace CarRace.Controllers
{
    public class EnemyController : IInitializable, IAsyncStartable, ITickable, IFixedTickable, IDisposable
    {
        private readonly EnemyFactory _enemyFactory;
        private readonly Transform _spawnPointsContainer;
        
        private List<StateMachine> _stateMachines;
        
        private CancellationTokenSource _cts;

        public EnemyController(EnemyFactory enemyFactory, LevelSceneContext levelSceneContext)
        {
            _enemyFactory = enemyFactory;
            _spawnPointsContainer = levelSceneContext.EnemySpawnPointsContainer;
        }

        public void Initialize()
        {
            _stateMachines  = new List<StateMachine>();
            
            _cts = new CancellationTokenSource();
        }

        public void Dispose()
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }
        
        public async UniTask StartAsync(CancellationToken cancellation = new CancellationToken())
        {
            await _enemyFactory.PrepareAsync(25, _cts.Token);
            
            SpawnEnemies();
        }
        
        public void Tick()
        {
            foreach (var stateMachine in _stateMachines)
            {
                stateMachine.Update();
            }
        }
        
        public void FixedTick()
        {
            foreach (var stateMachine in _stateMachines)
            {
                stateMachine.FixedUpdate();
            }
        }
        
        private void SpawnEnemies()
        {
            var childCount = _spawnPointsContainer.childCount;
            
            for (int i = 0; i < childCount; i++)
            {
                var childTransform = _spawnPointsContainer.GetChild(i);
                
                if(childTransform.gameObject.activeInHierarchy == false)
                {
                    continue;
                }
                
                var context = _enemyFactory.Get(childTransform.position);

                var stateMachine = new StateMachine();
            
                var states = new Dictionary<Type, BaseState>
                {
                    { typeof(EnemyIdleState), new EnemyIdleState(stateMachine, context) },
                    { typeof(EnemyFollowState), new EnemyFollowState(stateMachine, context) },
                    { typeof(EnemyAttackState), new EnemyAttackState(stateMachine, context) },
                    { typeof(DieState), new DieState(stateMachine, context) }
                };

                stateMachine.SetStates(typeof(EnemyIdleState), states);
                stateMachine.Start();

                _stateMachines.Add(stateMachine);
            }
        }
    }
}
