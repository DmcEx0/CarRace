using CarRace;
using CarRace.Helpers;
using CarRace.Test;
using UnityEngine;

public class EnemyFollowState : EnemyBaseState
{
    // Эти 3 поля будут дублироваться во всех стейтах из-за необходимости.
    // Поэтому надо будет их вынести в какой-нибудь TargetProvider.cs, чтобы передавать один экземпляр, вместо того, чтобы создавать новые в каждом стейте
    private readonly TargetSystem<TestPlayer> _targetSystem;
    private readonly LayerMask _layerMask = 1 << 7;

    public EnemyFollowState(IStateChanger stateChanger, EnemyContext context) : base(stateChanger, context)
    {
        _targetSystem = new TargetSystem<TestPlayer>(_layerMask, 1);
    }

    public override void OnEnter()
    {
        Context.View.Animancer.Play(Context.Config.FollowAnimation);
    }

    public override void OnUpdate()
    {
        var hasTarget = _targetSystem.TryGetNearest(out var target, Context.View.transform.position, Context.Config.FollowRadius);

        var transform = Context.View.transform;

        if (hasTarget)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Context.Config.Speed * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position);

            if ((target.transform.position - transform.position).magnitude < Context.Config.AttackRadius)
            {
                ChangeState(typeof(AttackState));
            }
        }
        else
        {
            ChangeState(typeof(EnemyIdleState));
        }
    }
}