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

    private Transform fordTr;
    private Transform playerTr;

    private float _speed;
    private float _followDistance = 0.1f;
    private float _lerpSpeed = 0.8f;

    private void Awake()
    {
        fordTr = GetComponent<Transform>();

        playerTr = GameObject.FindGameObjectWithTag("PlayerFordPos").transform;

        _stateMachine = gameObject.AddComponent<StateMachine>();
        
        _stateMachine.AddState(State.IDLE, new IdleState(this));
        _stateMachine.AddState(State.TRACE, new TraceState(this));
        _stateMachine.AddState(State.IDLEATTACK, new IdleAttackState(this));
        _stateMachine.AddState(State.TRACEATTACK, new TraceAttackState(this));
        _stateMachine.InitState(State.IDLE);
    }

    private void Start()
    {
        StartCoroutine(CoCheckFordState());
    }

    private IEnumerator CoCheckFordState()
    { 
        while (true)    // todo �÷��̾� ������� �̺�Ʈ ó���������� �޾ƿͼ� while�� �ȿ� ���� ����
        {
            yield return new WaitForSeconds(0.3f);

            if (true) // todo �÷��̾� ������� �̺�Ʈ ó���������� �޾ƿͼ� if�� �ȿ� ���� ����
            { 
                // todo
            }

            float distance = Vector3.Distance(fordTr.position, playerTr.position);

            bool test = true;
            if (test) // todo Ford ����Ű�� �Է� �޾��� ���� ���� �޾ƿͼ� true ����
            {
                if (distance < _followDistance)
                {
                    _stateMachine.ChangeState(State.IDLE);
                }
                else
                {
                    _stateMachine.ChangeState(State.TRACE);
                }
            }
            else
            {
                if (distance < _followDistance)
                {
                    _stateMachine.ChangeState(State.IDLEATTACK);
                }
                else
                { 
                    _stateMachine.ChangeState(State.TRACEATTACK);
                }
            }
        }
    }

    private void ChasePlayer()
    {
        transform.position = Vector3.Lerp(fordTr.position, playerTr.position, _lerpSpeed * Time.deltaTime);
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
        public override void FixedUpdate()
        {
            ford.ChasePlayer();
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
        public override void FixedUpdate()
        {
            ford.ChasePlayer();
        }
    }
}
