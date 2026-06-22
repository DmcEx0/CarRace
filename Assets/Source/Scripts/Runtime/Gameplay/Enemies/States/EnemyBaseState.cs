using CarRace.Gameplay.Enemies;
using CarRace.Gameplay.FSM;

namespace CarRace.Gameplay.Enemies.States
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
