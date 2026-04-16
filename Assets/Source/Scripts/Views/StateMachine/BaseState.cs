using System;
using CarRace;
using UnityEngine;

public abstract class BaseState
{
    private readonly IStateChanger _stateChanger;
        
    public BaseState(IStateChanger stateChanger)
    {
        _stateChanger = stateChanger;
    }

    public virtual void OnEnter()
    {
    }

    public virtual void OnUpdate()
    {
    }

    public virtual void OnFixedUpdate()
    {
    }
        
    public virtual void OnExit()
    {
    }
    
    protected void ChangeState(Type stateType)
    {
        _stateChanger.ChangeState(stateType);
    }
}

