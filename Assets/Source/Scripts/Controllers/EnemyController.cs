using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;

namespace CarRace
{
    public class EnemyController : IInitializable, IAsyncStartable, ITickable, IFixedTickable
    {
        private readonly EnemyFactory _enemyFactory;
        private readonly Transform _spawnPointContainer;
        
        private List<StateMachine> _stateMachines;

        public EnemyController(EnemyFactory enemyFactory, Transform spawnPointContainer)
        {
            _enemyFactory = enemyFactory;
            _spawnPointContainer = spawnPointContainer;
        }

        public void Initialize()
        {
            _stateMachines  = new List<StateMachine>();
        }

        public async UniTask StartAsync(CancellationToken cancellation = new CancellationToken())
        {
            await _enemyFactory.PrepareAsync(25);
            
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
            var childCount = _spawnPointContainer.childCount;
            
            for (int i = 0; i < childCount; i++)
            {
                var context = _enemyFactory.Get(_spawnPointContainer.GetChild(i).position);

                var stateMachine = new StateMachine();
            
                var states = new Dictionary<Type, BaseState>
                {
                    { typeof(EnemyIdleState), new EnemyIdleState(stateMachine, context) },
                    { typeof(EnemyFollowState), new EnemyFollowState(stateMachine, context) },
                    { typeof(AttackState), new AttackState(stateMachine, context) },
                    { typeof(DieState), new DieState(stateMachine, context) }
                };

                stateMachine.SetStates(typeof(EnemyIdleState), states);
                stateMachine.Start();

                _stateMachines.Add(stateMachine);
            }
        }
    }
}
