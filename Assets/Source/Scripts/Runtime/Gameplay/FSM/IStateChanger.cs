using System;

namespace CarRace.Gameplay.FSM
{
    public interface IStateChanger
    {
        public void ChangeState(Type stateType);
    }
}
