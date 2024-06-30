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
    private NavMeshAgent nav;
    private Animator anim;

    private Transform _playerTr;

    private float _maxHp;
    private float _hp;
    private float _damage;
    private float _sensingRange;
    private float _attackRange;
    private float _dropExp;

    private bool isDie = false;

    private readonly int hashTrace = Animator.StringToHash("Trace");
    private readonly int hashAttack = Animator.StringToHash("Attack");
    private readonly int hashDie = Animator.StringToHash("Die");

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
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
        StartCoroutine(CoEnemyState());
    }

    private IEnumerator CoEnemyState()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.3f);

            if (_hp <= 0)
            { 
                _stateMachine.ChangeState(State.DIE);
                Die();
                yield break;
            }

            float distance = Vector3.Distance(transform.position, _playerTr.position);

            if (distance <= _attackRange)
            { 
                _stateMachine.ChangeState(State.ATTACK);
            }
            else if (distance <= _sensingRange)
            {
                _stateMachine.ChangeState(State.TRACE);
            }
            else
            {
                _stateMachine.ChangeState(State.IDLE);
            }
        }
    }

    private void Die()
    { 
        
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
            enemy.nav.isStopped = true;
            enemy.anim.SetBool(enemy.hashTrace, false);

            enemy.state = State.IDLE;
        }
    }
    public class TraceState : BaseEnemyState
    { 
        public TraceState(EnemyController enemy) : base(enemy) { }
        public override void Enter()
        {
            enemy.nav.isStopped = false;
            enemy.anim.SetBool(enemy.hashTrace, true);

            enemy.state = State.TRACE;
        }
    }
    public class AttackState : BaseEnemyState
    {
        public AttackState(EnemyController enemy) : base(enemy) { }
        public override void Enter()
        {
            enemy.anim.SetBool(enemy.hashAttack, true);
            enemy.nav.isStopped = true;
            
            enemy.state = State.ATTACK;
        }
    }
    public class DieState : BaseEnemyState
    {
        public DieState(EnemyController enemy) : base(enemy) { }
        public override void Enter()
        {
            enemy.anim.SetTrigger(enemy.hashDie);

            enemy.state = State.DIE;
        }
    }
}
