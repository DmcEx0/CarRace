using System;
using System.Collections.Generic;
using UnityEngine;

namespace CarRace.FSM
{
    public class StateMachine : IStateChanger
    {
        private Dictionary<Type, BaseState> _states = new Dictionary<Type, BaseState>();
        private BaseState _currentState;
        private Type _startStateType;
        private bool _isStop;

        public IReadOnlyDictionary<Type, BaseState> States => _states;

        public void SetStates(Type startStateType, Dictionary<Type, BaseState> states)
        {
            _states = states ?? new Dictionary<Type, BaseState>();
            _startStateType = startStateType;

            if (_states.TryGetValue(startStateType, out var state))
            {
                if (_currentState == null)
                {
                    _currentState = state;
                }
            }
        }

        public void Start()
        {
            if (_states.TryGetValue(_startStateType, out var state))
            {
                if (_currentState == null)
                {
                    _currentState = state;
                }
            }

            _currentState?.OnEnter();
            _isStop = false;
        }

        public void Stop()
        {
            _currentState?.OnExit();
            _isStop = true;
        }

        public void Update()
        {
            if (_isStop)
            {
                return;
            }

            _currentState?.OnUpdate();
        }

        public void FixedUpdate()
        {
            if (_isStop)
            {
                return;
            }

            _currentState?.OnFixedUpdate();
        }

        public void ChangeState(Type stateType)
        {
            if (stateType != null && _states.TryGetValue(stateType, out BaseState state))
            {
                _currentState?.OnExit();
                _currentState = state;
                _currentState.OnEnter();
            }
            else
            {
                Debug.LogWarning($"State {stateType?.Name} not found in StateMachine.");
            }
        }
    }
}
