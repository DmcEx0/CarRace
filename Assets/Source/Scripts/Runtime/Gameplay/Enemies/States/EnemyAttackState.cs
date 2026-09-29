using CarRace.Gameplay.FSM;

namespace CarRace.Gameplay.Enemies.States
{
    public class EnemyAttackState : EnemyBaseState
    {
        private float _radius = 10;

        public EnemyAttackState(IStateChanger stateChanger, EnemyContext context) : base(stateChanger, context)
        {
        }

        public override void OnEnter()
        {
        }

        public override void OnUpdate()
        {
            var hasTarget =
                Context.SphereTargetFinder.TryGetNearest(Context.View.transform.position, _radius, out var target);

            if (hasTarget == false)
            {
                ChangeState(typeof(EnemyFollowState));
            }
        }

        public override void OnExit()
        {
        }
    }
}