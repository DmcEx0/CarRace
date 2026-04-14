using UnityEngine;

public class FollowState : BaseState
{
    public override void EnterState()
    {

    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        _stateMachine.View.transform.position = Vector3.MoveTowards(_stateMachine.View.transform.position, _stateMachine.PlayerPosition, 1f);
        if ((_stateMachine.PlayerPosition - _stateMachine.View.transform.position).magnitude < 0.5f)
        {
            _stateMachine.NextState = (int)States.Attack;
        }
    }
}
