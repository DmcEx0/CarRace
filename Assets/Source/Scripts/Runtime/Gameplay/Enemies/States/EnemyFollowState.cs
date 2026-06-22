using UnityEngine;

using CarRace.Gameplay.Enemies;
using CarRace.Gameplay.FSM;
using CarRace.Gameplay.Targeting;

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
            var hasTarget = Context.TargetFinder.TryGetNearest(Context.View.transform.position, Context.FollowRadius, out var target);

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
