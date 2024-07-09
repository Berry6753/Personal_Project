using System.Collections;
using System.Collections.Generic;
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
    private Vector3 _defaultPos;
    private Quaternion _defaultRot;
    private Vector3 _playerLookAt;

    [SerializeField] private BoxCollider _rAttackCollider;
    [SerializeField] private BoxCollider _lAttackCollider;
    [SerializeField] private BoxCollider _skillCollider;

    private ParticleSystem _skillAttack;

    [SerializeField] private string _name;

    [SerializeField] private float _maxHp;
    [SerializeField] private float _hp;
    public float _damage;
    [SerializeField] private float _attackRange;
    [SerializeField] private float _maxExp;
    [SerializeField] private float _minExp;
    private float _dropExp;

    private int _onAttackNum;

    private bool isDead;
    public bool isAttack;

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
        _defaultPos = transform.position;
        _defaultRot = transform.rotation;

        _skillAttack = ParticleManager.Instance.AOParticle;

        _stateMachine = gameObject.AddComponent<StateMachine>();

        _stateMachine.AddState(State.IDLE, new IdleState(this));
        _stateMachine.AddState(State.TRACE, new TraceState(this));
        _stateMachine.AddState(State.COMEBACK, new ComebackState(this));
        _stateMachine.AddState(State.ATTACK, new AttackState(this));
        _stateMachine.AddState(State.DIE, new DieState(this));
    }

    private void OnEnable()
    {
        _rAttackCollider.enabled = false;
        _lAttackCollider.enabled = false;
        _skillCollider.enabled = false;
        _hp = _maxHp;
        isDead = false;
        isAttack = false;
        transform.position = _defaultPos;
        transform.rotation = _defaultRot;
        _stateMachine.InitState(State.IDLE);
        StartCoroutine(CoAOState());
    }

    private void Update()
    {
        _playerLookAt = new Vector3(_playerTr.position.x, transform.position.y, _playerTr.position.z);
    }
    private IEnumerator CoAOState()
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

            if(backDistance <= 0.7f)
            {
                if (state == State.ATTACK) continue;
                if (state == State.TRACE) continue;

                _stateMachine.ChangeState(State.IDLE);
            }
        }
    }


    public void OnAttackThink()
    {
        //_onAttackNum = 3;
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

    private void RecoverHp()
    {
        if (_hp >= _maxHp)
        {
            _hp = _maxHp;
            return;
        }

        _hp += Time.deltaTime;
    }

    public void ChangeMoveState(string state)
    {
        switch(state)
        {
            case "Trace":
                _stateMachine.ChangeState(State.TRACE);
                break;
            case "ComeBack":
                _stateMachine.ChangeState(State.COMEBACK);
                break;
        }
        
    }

    public void EnableAttackCollider(string col)
    {
        switch (col)
        {
            case "R":
                _rAttackCollider.enabled = true;
                break;
            case "L":
                _lAttackCollider.enabled = true;
                break;
        }
    }
    public void DisableAttackCollider(string col)
    {
        switch (col)
        {
            case "R":
                _rAttackCollider.enabled = false;
                break;
            case "L":
                _lAttackCollider.enabled = false;
                break;
        }
    }
    private void EnableSkillCollider()
    { 
        _skillCollider.enabled = true;
    }
    private void DisableSkillCollider()
    {
        _skillCollider.enabled = false;
    }

    public void Hurt(float dmg)
    { 
        _hp -= dmg;
    }
    private void Die()
    {
        isDead = true;
        DropExp();
        Invoke("InActive", 5f);
    }
    private void DropExp()
    {
        _dropExp = Random.Range(_minExp, _maxExp);
        GameManager.Instance.PlayerGetExp(_dropExp);
    }
    private void InActive()
    {
        gameObject.SetActive(false);
    }

    private void PlayParticle()
    {
        Vector3 initPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        _skillAttack.transform.position = initPos;
        Invoke("OnPlayParticle", 2f);
    }
    private void OnPlayParticle()
    {
        _skillAttack.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerAttack"))
        {
            Hurt(GameManager.Instance.PlayerInfo._damage);
            Debug.Log("플레이어가 AO 공격");
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("SkillAttack"))
        {
            Hurt(GameManager.Instance.PlayerInfo._skillDamage);
            Debug.Log("플레이어가 AO 공격");
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            Hurt(GameManager.Instance.Ford.fordDmg);
            Debug.Log("포드가 AO 공격");
        }
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
            ao.transform.LookAt(null);
            ao.transform.rotation = ao._defaultRot;

            ao.state = State.IDLE;
        }
        public override void Update()
        {
            ao.RecoverHp();
        }
    }
    private class TraceState : BaseAOState
    { 
        public TraceState(AlphaOmegaController ao) : base(ao) { }
        public override void Enter()
        {
            ao.nav.isStopped = false;
            ao.anim.SetBool(ao.hashWalk, true);
            ao.transform.LookAt(ao._playerLookAt);

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
            ao.transform.LookAt(null);

            ao.state = State.COMEBACK;
        }
        public override void Update()
        {
            ao.nav.SetDestination(ao._defaultPos);

            ao.RecoverHp();
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
            //ao.anim.SetBool(ao.hashWalk, false);
            ao.anim.SetTrigger(ao.hashAttack);
            ao.OnAttack();
            ao.transform.LookAt(ao._playerLookAt);
            ao.isAttack = true;

            ao.state = State.ATTACK;
        }
        public override void Exit()
        {
            ao.anim.ResetTrigger(ao.hashAttack);
            ao.isAttack = false;
        }
    }
    private class DieState : BaseAOState
    { 
        public DieState(AlphaOmegaController ao) : base(ao) { }
        public override void Enter()
        {
            ao.nav.isStopped = true;
            ao.anim.SetTrigger(ao.hashDie);
            ao.transform.LookAt(null);
            ao.Die();

            ao.state = State.DIE;
        }
    }
}
