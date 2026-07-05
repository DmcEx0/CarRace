using CarRace.Gameplay.FSM;

namespace CarRace.Gameplay.Enemies.States
{
    public class EnemyFollowState : EnemyBaseState
    {
        public EnemyFollowState(IStateChanger stateChanger, EnemyContext context) : base(stateChanger, context) { }

        public override void OnEnter()
        {
            Context.View.Animancer.Play(Context.FollowAnimation);
        }

        public override void OnUpdate()
        {
            var hasTarget = Context.SphereTargetFinder.TryGetNearest(Context.View.transform.position, Context.FollowRadius, out var target);

            var agent = Context.View.Agent;

            if (hasTarget)
            {
                agent.SetDestination(target.transform.position);

                if ((target.transform.position - agent.transform.position).magnitude < Context.AttackRadius)
                {
                    ChangeState(typeof(EnemyAttackState));
                }
            }
            else
            {
                ChangeState(typeof(EnemyIdleState));
            }
        }

        public override void OnExit()
        {
            Context.View.Agent.SetDestination(Context.View.transform.position);
        }
    }
}
