using CarRace.Contexts;

namespace CarRace.FSM
{
    public class EnemyIdleState : EnemyBaseState
    {
        public EnemyIdleState(IStateChanger stateChanger, EnemyContext context) : base(stateChanger, context)
        {
        }

        public override void OnEnter()
        {
            Context.View.Animancer.Play(Context.IdleAnimation);
        }

        public override void OnUpdate()
        {
            var hasTarget = Context.TargetFinder.TryGetNearest(Context.View.transform.position, Context.FollowRadius,
                out var target);

            if (hasTarget)
            {
                ChangeState(typeof(EnemyFollowState));
            }
        }
    }
}