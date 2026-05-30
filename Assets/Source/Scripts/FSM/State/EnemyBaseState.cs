using CarRace.Contexts;

namespace CarRace.FSM
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
