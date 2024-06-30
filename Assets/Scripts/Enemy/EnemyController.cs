using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public enum State
    { 
        IDLE,
        TRACE,
        ATTACK,
        DIE
    }
    public State state = State.IDLE;
    private StateMachine _stateMachine;

    private Rigidbody rb;
    private NavMeshAgent agent;
    private Animator anim;

    private Transform _playerTr;

    private float _maxHp;
    private float _hp;
    private float _damage;
    private float _sensingRange;
    private float _attackRange;
    private float _dropExp;

    private bool isDie = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        _playerTr = GameObject.FindGameObjectWithTag("Player").transform;

        _stateMachine = gameObject.AddComponent<StateMachine>();

        _stateMachine.AddState(State.IDLE, new IdleState(this));
        _stateMachine.AddState(State.TRACE, new TraceState(this));
        _stateMachine.AddState(State.ATTACK, new AttackState(this));
        _stateMachine.AddState(State.DIE, new DieState(this));
        _stateMachine.InitState(State.IDLE);
    }

    private void Start()
    {
        StartCoroutine(CoEnemyMove());
    }

    private IEnumerator CoEnemyMove()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.3f);

            if (_hp <= 0)
            { 
                _stateMachine.ChangeState(State.DIE);
            }

            float distance = Vector3.Distance(transform.position, _playerTr.position);

            if (distance >= _recognitionDistance)
            {
                _stateMachine.ChangeState(State.IDLE);
            }
            else
            {
                _stateMachine.ChangeState(State.TRACE);
            }
        }
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
    public class DieState : BaseEnemyState
    {
        public DieState(EnemyController enemy) : base(enemy) { }
        public override void Enter()
        {
           
        }
    }
}
