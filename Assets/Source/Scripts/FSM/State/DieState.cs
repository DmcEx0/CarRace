using CarRace.Contexts;

namespace CarRace.FSM
{
    public class DieState : EnemyBaseState
    {
        public DieState(IStateChanger stateChanger, EnemyContext context) : base(stateChanger, context)
        {
        }
    }
}
