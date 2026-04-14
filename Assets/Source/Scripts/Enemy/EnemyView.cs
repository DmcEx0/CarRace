using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace CarRace
{
    public class EnemyView : MonoBehaviour
    {
        private StateMachine _stateMachine;

        [Inject]
        public void Construct(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        void Start()
        {
            _stateMachine.View = this;
        }
    }
}
