using CarRace.Contexts;
using UnityEngine;

namespace CarRace.FSM
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
            var hasTarget = Context.TargetSystem.TryGetNearest(out var target, Context.View.transform.position, Context.FollowRadius);

            var transform = Context.View.transform;

            if (hasTarget)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Context.Speed * Time.deltaTime);
                transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position);

                if ((target.transform.position - transform.position).magnitude < Context.AttackRadius)
                {
                    ChangeState(typeof(EnemyAttackState));
                }
            }
            else
            {
                ChangeState(typeof(EnemyIdleState));
            }
        }
    }
}
