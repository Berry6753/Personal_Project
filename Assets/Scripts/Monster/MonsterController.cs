using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
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
    protected StateMachine _stateMachine;

    protected Rigidbody rb;
    protected Animator anim;
    protected NavMeshAgent nav;

    private Transform _playerTr;
    protected Vector3 _defaultPos;
    protected Quaternion _defaultRot;
    protected Vector3 _playerLookAt;

    [SerializeField] protected float _maxHp;
    [SerializeField] protected float _hp;
    public float _damage;
    [SerializeField] protected float _attackRange;
    [SerializeField] protected float _maxExp;
    [SerializeField] protected float _minExp;
    protected float _dropExp;

    public Action<MonsterController> onDeath;
    public Action<MonsterController> disable;

    protected bool isDead = false;
    protected bool isAttack = false;

    protected readonly int hashAttack = Animator.StringToHash("isAttack");

    protected void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();

        _playerTr = GameObject.FindGameObjectWithTag("Player").transform;
        _defaultPos = transform.position;
        _defaultRot = transform.rotation;

        _stateMachine = gameObject.AddComponent<StateMachine>();

        _stateMachine.AddState(State.IDLE, new IdleState(this));
        _stateMachine.AddState(State.TRACE, new TraceState(this));
        _stateMachine.AddState(State.COMEBACK, new ComebackState(this));
        _stateMachine.AddState(State.ATTACK, new AttackState(this));
        _stateMachine.AddState(State.DIE, new DieState(this));

        GameManager.Instance.PlayerInfo.onDie += HandlerPlayerDie;
        disable += GameManager.Instance.HandlerMonsterDisable;
    }

    protected void OnEnable()
    {
        _hp = _maxHp;
        isDead = false;
        isAttack = false;
        transform.position = _defaultPos;
        transform.rotation = _defaultRot;
        _stateMachine.InitState(State.IDLE);
        StartCoroutine(CoState());
        nav.enabled = true;
    }

    protected IEnumerator CoState()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(0.3f);

            if (_hp <= 0)
            {
                _stateMachine.ChangeState(State.DIE);
                yield break;
            }

            float distance = Vector3.Distance(transform.position, _playerTr.position);

            if (distance <= _attackRange)
            {
                if (isAttack) continue;

                _stateMachine.ChangeState(State.ATTACK);
            }
            else
            {
                if (state == State.ATTACK && !isAttack)
                {
                    _stateMachine.ChangeState(State.TRACE);
                }
            }
            float backDistance = Vector3.Distance(transform.position, _defaultPos);

            if (backDistance <= 0.7f)
            {
                if (state == State.ATTACK) continue;
                if (state == State.TRACE) continue;

                _stateMachine.ChangeState(State.IDLE);
            }
        }
    }

    private void RecoverHp()
    {
        if (_hp >= _maxHp)
        {
            _hp = _maxHp;
            return;
        }

        _hp += Time.deltaTime;
    }

    public void Hurt(float dmg)
    {
        _hp -= dmg;
    }
    protected void Die()
    {
        isDead = true;
        DropExp();
        nav.enabled = false;
        onDeath?.Invoke(this);
        Invoke(nameof(InActive), 5f);
    }
    protected void DropExp()
    {
        _dropExp = UnityEngine.Random.Range(_minExp, _maxExp);
        GameManager.Instance.PlayerGetExp(_dropExp);
    }
    protected void InActive()
    {
        disable?.Invoke(this);
        gameObject.SetActive(false);
    }

    protected void HandlerPlayerDie(PlayerInfo info)
    {
        _stateMachine.ChangeState(State.COMEBACK);
    }

    public class MonsterState : BaseState
    {
        protected MonsterController monster;

        public MonsterState(MonsterController monster) 
        {  
            this.monster = monster; 
        }
    }
    protected class IdleState : MonsterState
    {
        public IdleState(MonsterController monster) : base(monster) { }

        public override void Enter()
        {
            monster.state = State.IDLE;
        }
    }
    protected class TraceState : MonsterState
    {
        public TraceState(MonsterController monster) : base(monster) { }

        public override void Enter()
        {
            monster.state = State.TRACE;
        }
    }
    protected class ComebackState : MonsterState
    { 
        public ComebackState(MonsterController monster) : base(monster) { }

        public override void Enter()
        {
            monster.state = State.COMEBACK;
        }
    }
    protected class AttackState : MonsterState
    { 
        public AttackState(MonsterController monster) : base(monster) { }

        public override void Enter()
        {
            monster.state = State.ATTACK;   
        }
    }
    protected class DieState : MonsterState
    {
        public DieState(MonsterController monster) : base(monster) { }

        public override void Enter()
        {
            monster.state = State.DIE;   
        }
    }
}
