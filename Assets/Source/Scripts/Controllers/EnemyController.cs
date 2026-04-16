using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer.Unity;

namespace CarRace
{
    public class EnemyController : IInitializable, IStartable, ITickable, IFixedTickable
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

        public void Start()
        {
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
                var enemyView = _enemyFactory.Get(_spawnPointContainer.GetChild(i).position);

                var stateMachine = new StateMachine();
            
                var states = new Dictionary<Type, BaseState>
                {
                    { typeof(IdleState), new IdleState(stateMachine, enemyView) },
                    { typeof(FollowState), new FollowState(stateMachine, enemyView) },
                    { typeof(AttackState), new AttackState(stateMachine, enemyView) },
                    { typeof(DieState), new DieState(stateMachine, enemyView) }
                };

                stateMachine.SetStates(typeof(IdleState), states);
                stateMachine.Start();

                _stateMachines.Add(stateMachine);
            }
        }
    }
}
