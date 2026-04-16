using System;

namespace CarRace
{
    public interface IStateChanger
    {
        public void ChangeState(Type stateType);
    }
}
