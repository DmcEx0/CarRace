using UnityEngine;

public class AttackState : BaseState
{
    public override void EnterState()
    {
        Debug.Log("Attack");
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        if ((_stateMachine.PlayerPosition - _stateMachine.View.transform.position).magnitude > 0.5f)
        {
            _stateMachine.NextState = (int)States.Follow;
        }
    }
}
