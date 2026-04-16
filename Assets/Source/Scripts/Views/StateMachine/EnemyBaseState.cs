using UnityEngine;

namespace CarRace
{
    public class EnemyBaseState : BaseState
    {
        protected EnemyView View { get; private set; }
        public EnemyBaseState(IStateChanger stateChanger, EnemyView view) : base(stateChanger)
        {
            View = view;
        }
    }
}
