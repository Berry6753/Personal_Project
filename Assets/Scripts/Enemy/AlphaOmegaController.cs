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
    [SerializeField] private float _sensingRange;
    [SerializeField] private float _attackRange;
    [SerializeField] private float _maxExp;
    [SerializeField] private float _minExp;
    private float _dropExp;

    private bool isDead = false;

    private readonly int hashTrace = Animator.StringToHash("isTrace");
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
        _stateMachine.AddState(State.ATTACK, new AttackState(this));
        _stateMachine.AddState(State.DIE, new DieState(this));
        _stateMachine.InitState(State.IDLE);
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
            
        }
    }
    private class TraceState : BaseAOState
    { 
        public TraceState(AlphaOmegaController ao) : base(ao) { }
        public override void Enter()
        {

        }
    }
    private class AttackState : BaseAOState
    {
        public AttackState(AlphaOmegaController ao) : base(ao) { }
        public override void Enter()
        {
            
        }
    }
    private class DieState : BaseAOState
    { 
        public DieState(AlphaOmegaController ao) : base(ao) { }
        public override void Enter()
        {
            
        }
    }
}
