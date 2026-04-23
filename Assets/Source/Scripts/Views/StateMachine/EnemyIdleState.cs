using CarRace;
using CarRace.Helpers;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    // Эти 3 поля будут дублироваться во всех стейтах из-за необходимости.
    // Поэтому надо будет их вынести в какой-нибудь TargetProvider.cs, чтобы передавать один экземпляр, вместо того, чтобы создавать новые в каждом стейте
    private readonly TargetSystem<Car> _targetSystem; 
    private readonly LayerMask _layerMask = 1 << 7;

    public EnemyIdleState(IStateChanger stateChanger, EnemyContext context) : base(stateChanger, context)
    {
        _targetSystem = new TargetSystem<Car>(_layerMask, 1);
    }

    public override void OnEnter()
    {
        Context.View.Animancer.Play(Context.Config.IdleAnimation);
    }

    public override void OnUpdate()
    {
        var hasTarget = _targetSystem.TryGetNearest(out var target, Context.View.transform.position,
            Context.Config.FollowRadius);

        if (hasTarget)
        {
            ChangeState(typeof(EnemyFollowState));
        }
    }
}