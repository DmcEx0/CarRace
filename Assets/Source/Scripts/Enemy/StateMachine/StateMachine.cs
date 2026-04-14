using System.Runtime.CompilerServices;
using CarRace;
using UnityEditorInternal;
using UnityEngine;
using VContainer;
using VContainer.Unity;
public class StateMachine : IInitializable, IStartable, ITickable
{
    private BaseState _idle;
    private BaseState _follow;
    private BaseState _attack;
    private BaseState _die;
    private BaseState[] _states = new BaseState[4];

    private Transform _playerTransform;

    public int NextState;
    public int CurrentState;
    public Vector3 PlayerPosition => _playerTransform.position;
    public EnemyView View;

    public StateMachine(IdleState idle, FollowState follow, AttackState attack,
        DieState die, [Key(TransformKey.PlayerTransform)] Transform playerTransform)
    {
        _states[(int)States.Idle] = idle;
        _states[(int)States.Follow] = follow;
        _states[(int)States.Attack] = attack;
        _states[(int)States.Die] = die;
        _playerTransform = playerTransform;
    }

    public void Start()
    {
        NextState = (int)States.Idle;
        CurrentState = (int)States.Idle;
        for (int i = 0; i < 4; i++)
        {
            _states[i].SetStateMachine(this);
        }
        _states[CurrentState].EnterState();
    }

    public void Tick()
    {
        Debug.Log(View.transform.position);
        if (NextState != CurrentState)
        {
            ChangeState();
        }
        _states[CurrentState].UpdateState();
    }

    public void ChangeState()
    {

        _states[CurrentState].ExitState();
        CurrentState = NextState;
        _states[CurrentState].EnterState();
    }
    
    public void Initialize()
    {
    }
}
public enum States
{
    Idle = 0,
    Follow = 1,
    Attack = 2,
    Die = 3
}
