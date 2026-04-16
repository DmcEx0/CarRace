using CarRace;
using CarRace.Helpers;
using UnityEngine;

public class IdleState : EnemyBaseState
{
    // Эти 3 поля будут дублироваться во всех стейтах из-за необходимости.
    // Поэтому надо будет их вынести в какой-нибудь TargetProvider.cs, чтобы передавать один экземпляр, вместо того, чтобы создавать новые в каждом стейте
    private readonly TargetSystem<Car> _targetSystem; 
    private readonly LayerMask _layerMask = 1 << 7;
    private float _radius = 10;
    
    public IdleState(IStateChanger stateChanger, EnemyView view) : base(stateChanger, view)
    {
        _targetSystem = new TargetSystem<Car>(_layerMask, 1);
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
