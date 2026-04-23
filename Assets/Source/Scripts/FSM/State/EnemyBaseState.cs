using UnityEngine;

namespace CarRace
{
    public class EnemyBaseState : BaseState
    {
        protected EnemyContext Context { get; private set; }
        public EnemyBaseState(IStateChanger stateChanger, EnemyContext context) : base(stateChanger)
        {
            Context = context;
        }
    }
}
