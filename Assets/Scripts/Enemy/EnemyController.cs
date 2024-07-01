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
    protected StateMachine _stateMachine;

    protected Rigidbody rb;
    protected NavMeshAgent nav;
    protected Animator anim;

    protected Transform _playerTr;

    protected float _maxHp;
    protected float _hp;
    protected float _damage;
    protected float _sensingRange;
    protected float _attackRange;
    protected float _dropExp;
    protected float _minExp;
    protected float _maxExp;

    protected bool isDie = false;

    protected readonly int hashTrace = Animator.StringToHash("Trace");
    protected readonly int hashAttack = Animator.StringToHash("Attack");
    protected readonly int hashDie = Animator.StringToHash("Die");

    protected void Awake()
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

    protected void Start()
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

    protected virtual void Attack()
    { 
        
    }

    protected void Die()
    { 
        isDie = true;
        DropExp();
    }
    protected void DropExp()
    { 
        _dropExp = Random.Range(_minExp, _maxExp);
        GameManager.Instance.PlayerGetExp(_dropExp);
    }

    public class BaseEnemyState : BaseState
    {
        protected EnemyController enemy;
        public BaseEnemyState(EnemyController enemy)
        { 
            this.enemy = enemy;
        }
    }
    protected class IdleState : BaseEnemyState
    {
        public IdleState(EnemyController enemy) : base(enemy) { }
        public override void Enter()
        {
            enemy.nav.isStopped = true;
            enemy.anim.SetBool(enemy.hashTrace, false);

            enemy.state = State.IDLE;
        }
    }
    protected class TraceState : BaseEnemyState
    { 
        public TraceState(EnemyController enemy) : base(enemy) { }
        public override void Enter()
        {
            enemy.nav.isStopped = false;
            enemy.anim.SetBool(enemy.hashTrace, true);

            enemy.state = State.TRACE;
        }
    }
    protected class AttackState : BaseEnemyState
    {
        public AttackState(EnemyController enemy) : base(enemy) { }
        public override void Enter()
        {
            enemy.anim.SetBool(enemy.hashAttack, true);
            enemy.Attack();
            enemy.nav.isStopped = true;
            
            enemy.state = State.ATTACK;
        }
    }
    protected class DieState : BaseEnemyState
    {
        public DieState(EnemyController enemy) : base(enemy) { }
        public override void Enter()
        {
            enemy.anim.SetTrigger(enemy.hashDie);
            enemy.Die();

            enemy.state = State.DIE;
        }
    }
}
