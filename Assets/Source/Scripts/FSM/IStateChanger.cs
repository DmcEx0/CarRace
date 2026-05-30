using System;

namespace CarRace.FSM
{
    public interface IStateChanger
    {
        public void ChangeState(Type stateType);
    }
}
