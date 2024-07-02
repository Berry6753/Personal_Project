using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;

public class AlphaOmegaController : MonoBehaviour
{
    public enum State
    { 
        IDLE,
        TRACE,
        COMEBACK,
        ATTACK,
        DIE
    }
    public State state = State.IDLE;
    private StateMachine _stateMachine;

    private Rigidbody rb;
    private Animator anim;
    private NavMeshAgent nav;

    private Transform _playerTr;

    [SerializeField] private string _name;

    [SerializeField] private float _maxHp;
    [SerializeField] private float _hp;
    [SerializeField] private float _damage;
    [SerializeField] private float _attackRange;
    [SerializeField] private float _maxExp;
    [SerializeField] private float _minExp;
    private float _dropExp;

    private int _onAttackNum;

    private bool isDead = false;

    private readonly int hashWalk = Animator.StringToHash("isWalk");
    private readonly int hashAttack = Animator.StringToHash("isAttack");
    private readonly int hashAttackNum = Animator.StringToHash("Attack");
    private readonly int hashDie = Animator.StringToHash("isDie");

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();

        _playerTr = GameObject.FindGameObjectWithTag("Player").transform;

        _stateMachine = gameObject.AddComponent<StateMachine>();

        _stateMachine.AddState(State.IDLE, new IdleState(this));
        _stateMachine.AddState(State.TRACE, new TraceState(this));
        _stateMachine.AddState(State.COMEBACK, new ComebackState(this));
        _stateMachine.AddState(State.ATTACK, new AttackState(this));
        _stateMachine.AddState(State.DIE, new DieState(this));
        _stateMachine.InitState(State.IDLE);
    }

    private void Start()
    {
        StartCoroutine(CoAOState());
    }
    private IEnumerator CoAOState()
    { 
        while (true)
        {
            yield return new WaitForSeconds(0.3f);

            if(_hp<=0)
            {
                _stateMachine.ChangeState(State.DIE);    
                yield break;
            }

            float distance = Vector3.Distance(transform.position, _playerTr.position);

            if (distance <= _attackRange)
            { 
                _stateMachine.ChangeState(State.ATTACK);
            }
            else
            {
                if (state == State.TRACE) continue;
                if (state == State.COMEBACK) continue;

                _stateMachine.ChangeState(State.IDLE);
            }
        }
    }



    public void OnAttackThink()
    {
        _onAttackNum = Random.Range(0, 5);
    }
    private void OnAttack()
    { 
        if(_onAttackNum <3)
        {
            anim.SetInteger(hashAttackNum, 0);
        }
        else if( _onAttackNum ==3)
        {
            anim.SetInteger(hashAttackNum, 1);
        }
        else
        {
            anim.SetInteger(hashAttackNum, 2);
        }
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            _stateMachine.ChangeState(State.TRACE);
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
            _stateMachine.ChangeState(State.COMEBACK);
    }

    public class BaseAOState : BaseState
    {
        protected AlphaOmegaController ao;
        public BaseAOState(AlphaOmegaController ao)
        {
            this.ao = ao;
        }
    }
    private class IdleState : BaseAOState
    { 
        public IdleState(AlphaOmegaController ao) : base(ao) { }
        public override void Enter()
        {
            ao.nav.isStopped = true;
            ao.anim.SetBool(ao.hashWalk, false);

            ao.state = State.IDLE;
        }
    }
    private class TraceState : BaseAOState
    { 
        public TraceState(AlphaOmegaController ao) : base(ao) { }
        public override void Enter()
        {
            ao.nav.isStopped = false;
            ao.anim.SetBool(ao.hashWalk, true);

            ao.state = State.TRACE;
        }
        public override void Update()
        {
            ao.nav.SetDestination(ao._playerTr.position);
        }
        public override void Exit()
        {
            ao.nav.isStopped = true;
        }
    }
    private class ComebackState : BaseAOState
    { 
        public ComebackState(AlphaOmegaController ao) : base(ao) { }
        public override void Enter()
        {
            ao.nav.isStopped = false;
            ao.anim.SetBool(ao.hashWalk, true);
        }
        public override void Update()
        {
            ao.nav.SetDestination(ao._playerTr.position);
        }
        public override void Exit()
        {
            ao.nav.isStopped = true;
        }
    }
    private class AttackState : BaseAOState
    {
        public AttackState(AlphaOmegaController ao) : base(ao) { }
        public override void Enter()
        {
            ao.nav.isStopped = true;
            ao.anim.SetTrigger(ao.hashAttack);
            ao.OnAttack();

            ao.state = State.ATTACK;
        }
    }
    private class DieState : BaseAOState
    { 
        public DieState(AlphaOmegaController ao) : base(ao) { }
        public override void Enter()
        {
            ao.anim.SetTrigger(ao.hashDie);
            ao.Die();

            ao.state = State.DIE;
        }
    }
}
