using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public enum State
    { 
        IDLE,
        TRACE,
        ATTACK
    }
    public State state = State.IDLE;
    private StateMachine _stateMachine;

    private void Awake()
    {
        _stateMachine = gameObject.AddComponent<StateMachine>();

        _stateMachine.AddState(State.IDLE, new IdleState(this));
        _stateMachine.AddState(State.TRACE, new TraceState(this));
        _stateMachine.AddState(State.ATTACK, new AttackState(this));
        _stateMachine.InitState(State.IDLE);
    }



    public class BaseEnemyState : BaseState
    {
        protected EnemyController enemy;
        public BaseEnemyState(EnemyController enemy)
        { 
            this.enemy = enemy;
        }
    }
    public class IdleState : BaseEnemyState
    {
        public IdleState(EnemyController enemy) : base(enemy) { }
        public override void Enter()
        {
            
        }
    }
    public class TraceState : BaseEnemyState
    { 
        public TraceState(EnemyController enemy) : base(enemy) { }
        public override void Enter()
        {

        }
    }
    public class AttackState : BaseEnemyState
    {
        public AttackState(EnemyController enemy) : base(enemy) { }
        public override void Enter()
        {

        }
    }
}
