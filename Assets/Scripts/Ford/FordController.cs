using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FordController : MonoBehaviour
{
    public enum State
    {
        IDLE,
        TRACE,
        IDLEATTACK,
        TRACEATTACK
    }
    public State state = State.IDLE;
    private StateMachine _stateMachine;

    private Rigidbody rb;
    private Transform playerTr;

    private float _speed;
    private float _distance;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        playerTr = GameObject.FindGameObjectWithTag("Player").transform;

        _stateMachine = gameObject.AddComponent<StateMachine>();
        
        _stateMachine.AddState(State.IDLE, new IdleState(this));
        _stateMachine.AddState(State.TRACE, new TraceState(this));
        _stateMachine.AddState(State.IDLEATTACK, new IdleAttackState(this));
        _stateMachine.AddState(State.TRACEATTACK, new TraceAttackState(this));
        _stateMachine.InitState(State.IDLE);
    }



    private class BaseFordState : BaseState
    {
        protected FordController ford;
        public BaseFordState(FordController ford)
        {
            this.ford = ford;
        }
    }
    private class IdleState : BaseFordState
    {
        public IdleState(FordController ford) : base(ford) { }
        public override void Enter()
        {

        }
    }
    private class TraceState : BaseFordState
    { 
        public TraceState(FordController ford) : base(ford) { }
        public override void Enter()
        {

        }
    }
    private class IdleAttackState : BaseFordState
    { 
        public IdleAttackState(FordController ford) : base(ford) { }
        public override void Enter()
        {
            
        }
    }
    private class TraceAttackState : BaseFordState
    { 
        public TraceAttackState(FordController ford) : base(ford) { }
        public override void Enter()
        {
            
        }
    }
}
