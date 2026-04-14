using UnityEngine;

public abstract class BaseState
{
    protected StateMachine _stateMachine;  
    public virtual void EnterState()
    {
        
    }
    public virtual void UpdateState()
    {
        
    }
    public virtual void ExitState()
    {
        
    }

    public void SetStateMachine(StateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }
}

