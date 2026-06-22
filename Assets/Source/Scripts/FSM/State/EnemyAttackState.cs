using CarRace.Contexts;

namespace CarRace.FSM
{
    public class EnemyAttackState : EnemyBaseState
    {
        private float _radius = 10;

        public EnemyAttackState(IStateChanger stateChanger, EnemyContext context) : base(stateChanger, context) { }

        public override void OnEnter()
        {
        }

        public override void OnUpdate()
        {
            var hasTarget = Context.TargetFinder.TryGetNearest(Context.View.transform.position, _radius, out var target);

            if(hasTarget)
            {
                if((target.transform.position - Context.View.transform.position).magnitude < 10)
                {
                    ChangeState(typeof(EnemyFollowState));
                }
            }
        }
    }
}
