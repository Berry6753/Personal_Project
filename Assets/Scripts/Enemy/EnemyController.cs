using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public enum State
    { 
        PATROL,
        TRACE,
        ATTACK,
        DIE
    }
    public State state = State.PATROL;
    protected StateMachine _stateMachine;

    protected Rigidbody rb;
    protected NavMeshAgent nav;
    protected Animator anim;

    protected Transform _playerTr;
    [SerializeField] protected List<Transform> _patrolTrList;

    [SerializeField] protected float _maxHp;
    [SerializeField] protected float _hp;
    [SerializeField] protected float _damage;
    [SerializeField] protected float _sensingRange;
    [SerializeField] protected float _attackRange;
    protected float _dropExp;
    [SerializeField] protected float _minExp;
    [SerializeField] protected float _maxExp;

    [SerializeField] protected int _nonCombatMoveThink;
    [SerializeField] protected int _nonCombatMoveTr;
    [SerializeField] protected int _nonCombatMoveTime;  

    protected bool isDead = false;

    protected readonly int hashCombat = Animator.StringToHash("isCombat");
    protected readonly int hashNonCombat = Animator.StringToHash("NonCombat");
    protected readonly int hashAttack = Animator.StringToHash("isAttack");
    protected readonly int hashAttackNumber = Animator.StringToHash("Attack");
    protected readonly int hashDie = Animator.StringToHash("isDie");

    protected int _nonCombatMove;
    protected int _AttackNumber;

    protected void Awake()
    {
        rb = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        _playerTr = GameObject.FindGameObjectWithTag("Player").transform;

        _stateMachine = gameObject.AddComponent<StateMachine>();

        _stateMachine.AddState(State.PATROL, new PatrolState(this));
        _stateMachine.AddState(State.TRACE, new TraceState(this));
        _stateMachine.AddState(State.ATTACK, new AttackState(this));
        _stateMachine.AddState(State.DIE, new DieState(this));
        _stateMachine.InitState(State.PATROL);
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
                if (state == State.PATROL) yield break;

                _stateMachine.ChangeState(State.PATROL);
            }
        }
    }

    protected void NonCombatThink()
    {
        _nonCombatMoveThink = Random.Range(0, 2);
        _nonCombatMoveTr = Random.Range(0, _patrolTrList.Count);
        _nonCombatMoveTime = Random.Range(2, 5);
    }
    protected void NonCombatMove()
    {
        NonCombatThink();
        if (_nonCombatMoveThink == 0)
        {
            nav.isStopped = true;
            Invoke("NonCombatMove", 2.0f);
        }
        else
        { 
            nav.isStopped = false;
            nav.SetDestination(_patrolTrList[_nonCombatMoveTr].position);
            Invoke("NonCombatMove", 5.0f);
        }
    }

    protected virtual void Attack()
    { 
        
    }

    protected void Die()
    {
        isDead = true;
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
    protected class PatrolState : BaseEnemyState
    {
        public PatrolState(EnemyController enemy) : base(enemy) { }
        public override void Enter()
        {
            enemy.anim.SetBool(enemy.hashCombat, false);
            enemy.NonCombatMove();

            enemy.state = State.PATROL;
        }
        public override void Update()
        {
            if (enemy.nav.velocity == Vector3.zero)
                enemy.anim.SetInteger(enemy.hashNonCombat, 0);
            else
                enemy.anim.SetInteger(enemy.hashNonCombat, 1);
        }
    }
    protected class TraceState : BaseEnemyState
    { 
        public TraceState(EnemyController enemy) : base(enemy) { }
        public override void Enter()
        {
            enemy.nav.isStopped = false;
            enemy.anim.SetBool(enemy.hashCombat, true);

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
