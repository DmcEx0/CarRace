using CarRace.Contexts;
using CarRace.Helpers;
using CarRace.Placeholders;
using CarRace.Utils;
using CarRace.Views;
using UnityEngine;

namespace CarRace.FSM
{
    public class EnemyIdleState : EnemyBaseState
    {
        private readonly LayerMask _layerMask = 1 << 7;

        public EnemyIdleState(IStateChanger stateChanger, EnemyContext context) : base(stateChanger, context) { }

        public override void OnEnter()
        {
            Context.View.Animancer.Play(Context.IdleAnimation);
        }

        public override void OnUpdate()
        {
            var hasTarget = Context.TargetSystem.TryGetNearest(out var target, Context.View.transform.position,
                Context.FollowRadius);

            if (hasTarget)
            {
                ChangeState(typeof(EnemyFollowState));
            }
        }
    }
}
