using CarRace;
using CarRace.Helpers;
using CarRace.Test;
using UnityEngine;

public class AttackState : EnemyBaseState
{
    // Эти 3 поля будут дублироваться во всех стейтах из-за необходимости.
    // Поэтому надо будет их вынести в какой-нибудь TargetProvider.cs, чтобы передавать один экземпляр, вместо того, чтобы создавать новые в каждом стейте
    private readonly TargetSystem<TestPlayer> _targetSystem; 
    private readonly LayerMask _layerMask = 1 << 7;
    private float _radius = 10;
    
    public AttackState(IStateChanger stateChanger, EnemyView view) : base(stateChanger, view)
    {
        _targetSystem = new TargetSystem<TestPlayer>(_layerMask, 1);
    }

    public override void OnEnter()
    {
        Debug.Log("Attack");
    }

    public override void OnUpdate()
    {
        var hasTarget = _targetSystem.TryGetNearest(out var target, View.transform.position, _radius);
        
        if(hasTarget)
        {
            if((target.transform.position - View.transform.position).magnitude < 10)
            {
                ChangeState(typeof(FollowState));
            }
        }
    }
}