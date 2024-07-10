using System;
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
    private Vector3 _defaultPos;
    private Quaternion _defaultRot;
    [SerializeField] private List<Transform> _patrolTrList;

    private Vector3 _playerLookAt;

    [SerializeField] private BoxCollider R_AttackCollider;
    [SerializeField] private BoxCollider L_AttackCollider;
    private ParticleSystem _skillAttack;

    [SerializeField] private string _name;
    [SerializeField] private float _maxHp;
    [SerializeField] private float _hp;
    public float _damage;
    [SerializeField] private float _sensingRange;
    [SerializeField] private float _attackRange;
    [SerializeField] private float _minExp;
    [SerializeField] private float _maxExp; 
    private float _dropExp;
    private float _defaultSpeed = 3.5f;
    private float _runSpeed = 5.0f;
    private float _dashSpeed = 7.0f;
    private float _timer = 0f;

    [SerializeField] private int _nonCombatMoveTr;
    [SerializeField] private int _onCombatMove;
    [SerializeField] private int _onAttackNum;

    public Action<WicklineController> onDeath;
    public Action<WicklineController> disable;
    private bool isDead = false;
    [SerializeField] private bool isInvoked;
    private bool isSkill = false;
    public bool isAttack = false;

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
        _defaultPos = transform.position;
        _defaultRot = transform.rotation;

        _skillAttack = ParticleManager.Instance.WicklineParticle;

        _stateMachine = gameObject.AddComponent<StateMachine>();

        _stateMachine.AddState(State.PATROL, new PatrolState(this));
        _stateMachine.AddState(State.TRACE, new TraceState(this));
        _stateMachine.AddState(State.ATTACK, new AttackState(this));
        _stateMachine.AddState(State.DIE, new DieState(this));

        GameManager.Instance.PlayerInfo.onDie += HandlerPlayerDie;
    }

    private void OnEnable()
    {
        R_AttackCollider.enabled = false;
        L_AttackCollider.enabled = false;
        _hp = _maxHp;
        isDead = false;
        isAttack = false;
        transform.position = _defaultPos;
        transform.rotation = _defaultRot;
        _stateMachine.InitState(State.PATROL);
        StartCoroutine(CoEnemyState());
        nav.enabled = true;
    }
    private void Update()
    {
        _playerLookAt = new Vector3(_playerTr.position.x, transform.position.y, _playerTr.position.z);
    }

    private IEnumerator CoEnemyState()
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
            else if (distance >= _sensingRange)
            {
                if (state == State.PATROL) continue;

                _stateMachine.ChangeState(State.PATROL);
            }
            else
            {
                if (state == State.PATROL)
                {
                    if (state == State.TRACE) continue;

                    _stateMachine.ChangeState(State.TRACE);
                }
                if (state == State.ATTACK && !isAttack)
                {
                    if (state == State.TRACE) continue;

                    _stateMachine.ChangeState(State.TRACE);
                }
            }
        }
    }

    private void NonCombatThink()
    {
        int previousMoveTr = _nonCombatMoveTr;
        while (previousMoveTr == _nonCombatMoveTr)
        {
            _nonCombatMoveTr = UnityEngine.Random.Range(0, _patrolTrList.Count);
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
        _onCombatMove = UnityEngine.Random.Range(0, 4);
    }
    private void OnCombatMove()
    {
        OnCombatThink();
        if (_onCombatMove == 0)
        {
            anim.SetInteger(hashOnCombat, 0);
            nav.speed = _defaultSpeed;
        }
        else
        {
            anim.SetInteger(hashOnCombat, 1);
            nav.speed = _runSpeed;
        }
        Invoke("OnCombatMove", 10.0f);      
    }
    private void OnSkill()
    {
        isSkill = true;
        CancelInvoke("OnCombatMove");
        anim.SetTrigger(hashSkill);
        nav.speed = _dashSpeed;
    }
    private void OnBehind()
    {
        isSkill = false;
        anim.SetTrigger(hashBehind);
        transform.position = _playerTr.position + Vector3.back * _attackRange;
        transform.rotation = _playerTr.rotation;
        transform.LookAt(_playerLookAt);
    }


    public void OnAttackThink()
    { 
        _onAttackNum = UnityEngine.Random.Range(0, 10);
    }
    private void Attack()
    { 
        if (_onAttackNum < 4)
        {
            anim.SetInteger(hashAttackNumber, 0);
        }
        else if (_onAttackNum < 8)
        {
            anim.SetInteger(hashAttackNumber, 1);
        }
        else if (_onAttackNum == 9)
        {
            anim.SetInteger(hashAttackNumber, 2);
        }
        else
        {
            anim.SetInteger(hashAttackNumber, 3);
        }
    }
    public void Enable_RightCollider()
    { 
        R_AttackCollider.enabled = true;
    }
    public void Disable_RightCollider()
    {
        R_AttackCollider.enabled = false;
    }
    public void Enable_LeftCollider()
    {
        L_AttackCollider.enabled = true;
    }
    public void Disable_LeftCollider()
    {
        L_AttackCollider.enabled = false;
    }

    public void Hurt(float dmg)
    {
        _hp -= dmg;
    }
    private void Die()
    {
        isDead = true;
        DropExp();
        nav.enabled = false;
        onDeath?.Invoke(this);
        Invoke("InActive", 5f);
    }
    private void DropExp()
    { 
        _dropExp = UnityEngine.Random.Range(_minExp, _maxExp);
        GameManager.Instance.PlayerGetExp(_dropExp);
    }
    private void InActive()
    {
        disable?.Invoke(this);  
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

    private void HandlerPlayerDie(PlayerInfo info)
    {
        _stateMachine.ChangeState(State.PATROL);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerAttack"))
        {
            Hurt(GameManager.Instance.PlayerInfo._damage);
            Debug.Log("플레이어가 위클라인 공격");
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("SkillAttack"))
        {
            Hurt(GameManager.Instance.PlayerInfo._skillDamage);
            Debug.Log("플레이어가 위클라인 공격");
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        { 
            Hurt(GameManager.Instance.Ford.fordDmg);
            Debug.Log("포드가 위클라인 공격");
        }
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
            enemy.transform.LookAt(null);
            enemy.anim.SetBool(enemy.hashCombat, false);
            enemy.nav.speed = enemy._defaultSpeed;
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
            enemy.OnCombatMove();
            enemy._timer = 0;

            enemy.state = State.TRACE;
        }
        public override void Update()
        {
            enemy.nav.SetDestination(enemy._playerTr.position);

            if (!enemy.isSkill)
            {
                enemy._timer += Time.deltaTime;
            }

            if (enemy._timer > 10.0f)
            {
                enemy.OnSkill();
            }

            float distance = Vector3.Distance(enemy.transform.position, enemy._playerTr.position);
            if (distance <= enemy._attackRange + 3 && enemy.isSkill)
            {
                enemy.OnBehind();
                enemy._timer = 0;
            }
        }
        public override void Exit()
        {
            enemy.nav.isStopped = true;
        }
    }
    private class AttackState : BaseEnemyState
    {
        public AttackState(WicklineController enemy) : base(enemy) { }
        public override void Enter()
        {
            enemy.anim.SetBool(enemy.hashCombat, false);
            enemy.anim.SetTrigger(enemy.hashAttack);
            enemy.Attack();
            enemy.transform.LookAt(enemy._playerLookAt);
            enemy.nav.isStopped = true;
            enemy.isAttack = true;
            
            enemy.state = State.ATTACK;
        }
        public override void Exit()
        {
            enemy.anim.ResetTrigger(enemy.hashAttack);
            enemy.isAttack = false;
        }
    }
    private class DieState : BaseEnemyState
    {
        public DieState(WicklineController enemy) : base(enemy) { }
        public override void Enter()
        {
            enemy.transform.LookAt(null);
            enemy.anim.SetTrigger(enemy.hashDie);
            enemy.Die();

            enemy.state = State.DIE;
        }
    }
}
