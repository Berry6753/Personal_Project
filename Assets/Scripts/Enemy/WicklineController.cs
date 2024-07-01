using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WicklineController : MonoBehaviour
{
    public enum State
    { 
        PATROL,
        TRACE,
        ATTACK,
        DIE
    }
    public State state = State.PATROL;
    private StateMachine _stateMachine;

    private Rigidbody rb;
    private NavMeshAgent nav;
    private Animator anim;

    private Transform _playerTr;
    [SerializeField] private List<Transform> _patrolTrList;

    [SerializeField] private string _name;
    [SerializeField] private float _maxHp;
    [SerializeField] private float _hp;
    [SerializeField] private float _damage;
    [SerializeField] private float _sensingRange;
    [SerializeField] private float _attackRange;
    private float _dropExp;
    [SerializeField] private float _minExp;
    [SerializeField] private float _maxExp;

    [SerializeField] private int _nonCombatMoveTr;
    [SerializeField] private int _onCombatMove;
    [SerializeField] private int _onAttackNum;

    private bool isDead = false;
    [SerializeField] private bool isInvoked;

    private readonly int hashCombat = Animator.StringToHash("isCombat");
    private readonly int hashNonCombat = Animator.StringToHash("NonCombat");
    private readonly int hashOnCombat = Animator.StringToHash("OnCombat");
    private readonly int hashSkill = Animator.StringToHash("isSkill");
    private readonly int hashBehind = Animator.StringToHash("isBehind");
    private readonly int hashAttack = Animator.StringToHash("isAttack");
    private readonly int hashAttackNumber = Animator.StringToHash("Attack");
    private readonly int hashDie = Animator.StringToHash("isDie");

    private void Awake()
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
                if (state == State.PATROL) yield break;

                _stateMachine.ChangeState(State.PATROL);
            }
        }
    }

    private void NonCombatThink()
    {
        int previousMoveTr = _nonCombatMoveTr;
        while (previousMoveTr == _nonCombatMoveTr)
        {
            _nonCombatMoveTr = Random.Range(0, _patrolTrList.Count);
        }
    }
    private void NonCombatMove()
    {
        NonCombatThink();
        nav.isStopped = false;
        nav.SetDestination(_patrolTrList[_nonCombatMoveTr].position);
    }

    private void OnCombatThink()
    {
        _onCombatMove = Random.Range(0, 5);
    }
    private void OnCombatMove()
    {
        if (_onCombatMove == 0)
        {
            anim.SetInteger(hashOnCombat, 0);
        }
        else if (_onCombatMove == 1)
        {
            OnSkill();
        }
        else
        {
            anim.SetInteger(hashOnCombat, 1);
        }

        if (_onCombatMove == 1) return;

        Invoke("OnCombatThink", 5.0f);
    }
    private void OnSkill()
    {
        anim.SetTrigger(hashSkill);
    }
    private void OnBehind()
    {
        anim.SetTrigger(hashBehind);
        transform.rotation = _playerTr.rotation;
        transform.position = _playerTr.position + Vector3.back * _attackRange;
    }


    private void OnAttackThink()
    { 
        
    }
    private void Attack()
    { 
        
    }

    private void Die()
    {
        isDead = true;
        DropExp();
    }
    private void DropExp()
    { 
        _dropExp = Random.Range(_minExp, _maxExp);
        GameManager.Instance.PlayerGetExp(_dropExp);
    }

    public class BaseEnemyState : BaseState
    {
        protected WicklineController enemy;
        public BaseEnemyState(WicklineController enemy)
        { 
            this.enemy = enemy;
        }
    }
    private class PatrolState : BaseEnemyState
    {
        public PatrolState(WicklineController enemy) : base(enemy) { }
        public override void Enter()
        {
            enemy.anim.SetBool(enemy.hashCombat, false);
            enemy.isInvoked = false;

            enemy.state = State.PATROL;
        }
        public override void Update()
        {
            if (enemy.nav.velocity == Vector3.zero)
            {
                enemy.anim.SetInteger(enemy.hashNonCombat, 0);
                if (!enemy.isInvoked)
                {
                    enemy.Invoke("NonCombatMove", 5f);
                    enemy.isInvoked = true;
                }
            }
            else
            {
                enemy.anim.SetInteger(enemy.hashNonCombat, 1);
                enemy.isInvoked = false;
            }
        }
    }
    private class TraceState : BaseEnemyState
    { 
        public TraceState(WicklineController enemy) : base(enemy) { }
        public override void Enter()
        {
            enemy.nav.isStopped = false;
            enemy.anim.SetBool(enemy.hashCombat, true);
            enemy.nav.SetDestination(enemy._playerTr.position);
            enemy.OnCombatThink();

            enemy.state = State.TRACE;
        }
        public override void Update()
        {
            enemy.OnCombatMove();

            float distance = Vector3.Distance(enemy.transform.position, enemy._playerTr.position);
            if (distance <= enemy._attackRange + 3 && enemy._onCombatMove == 1)
            {
                enemy.OnBehind();
            }
        }
    }
    private class AttackState : BaseEnemyState
    {
        public AttackState(WicklineController enemy) : base(enemy) { }
        public override void Enter()
        {
            enemy.anim.SetTrigger(enemy.hashAttack);
            enemy.Attack();
            enemy.nav.isStopped = true;
            
            enemy.state = State.ATTACK;
        }
    }
    private class DieState : BaseEnemyState
    {
        public DieState(WicklineController enemy) : base(enemy) { }
        public override void Enter()
        {
            enemy.anim.SetTrigger(enemy.hashDie);
            enemy.Die();

            enemy.state = State.DIE;
        }
    }
}
