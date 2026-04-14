using UnityEngine;

public class IdleState : BaseState
{

    public override void EnterState()
    {
    }

    public override void ExitState()
    {
        
    }

    public override void UpdateState()
    {
        if((_stateMachine.PlayerPosition - _stateMachine.View.transform.position).magnitude < 10)
        {
            _stateMachine.NextState = (int)States.Follow;
        }
    }

    
}
