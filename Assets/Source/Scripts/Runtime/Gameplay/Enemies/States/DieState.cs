using CarRace.Gameplay.FSM;

namespace CarRace.Gameplay.Enemies.States
{
    public class DieState : EnemyBaseState
    {
        public DieState(IStateChanger stateChanger, EnemyContext context) : base(stateChanger, context)
        {
        }
    }
}
