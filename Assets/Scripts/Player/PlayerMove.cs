using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting;

public class PlayerMove : MonoBehaviour
{
    public enum State
    { 
        IDLE,
        RUN,
        JUMP,
        DASH,
        ATTACK,
        GAURD,
        DIE
    }
    public State state = State.IDLE;

    private Rigidbody rb;
    private Animator anim;

    [SerializeField] private StateMachine _stateMachine;

    private readonly int hashRun = Animator.StringToHash("isRun");
    private readonly int hashJump = Animator.StringToHash("isJump");
    private readonly int hashDash = Animator.StringToHash("isDash");
    private readonly int hashAttack = Animator.StringToHash("isAttack");
    private readonly int hashGaurd = Animator.StringToHash("isGaurd");
    private readonly int hashDie = Animator.StringToHash("isDie");

    private Vector3 inputMoveMent = Vector3.zero;

    private float _moveSpeed = 5.0f;
    private float _rotSpeed = 10.0f;
    private float _dashSpeed = 20.0f;
    private float _jumpSpeed;

    private float _dashTime = 0.5f;
    private float _dashCoolDown = 0.2f;

    private int _jumpCount;
    private int _attackCount;

    private bool _isMove = false;
    private bool _isDash = false;
    private bool _isJump = false;
    private bool _isAttack = false;
    private bool _isDie = false;
    private bool _isInput = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        _stateMachine = gameObject.AddComponent<StateMachine>();

        _stateMachine.AddState(State.IDLE, new IdleState(this));
        _stateMachine.AddState(State.RUN, new RunState(this));
        _stateMachine.AddState(State.JUMP, new JumpState(this));
        _stateMachine.AddState(State.DASH, new DashState(this));
        _stateMachine.AddState(State.ATTACK, new AttackState(this));
        _stateMachine.AddState(State.GAURD, new GaurdState(this));
        _stateMachine.AddState(State.DIE, new DieState(this));
        _stateMachine.InitState(State.IDLE);
    }

    private void Update()
    {
        Debug.Log(state); 
    }

    public void OnMove_Player(InputAction.CallbackContext context)
    {
        if (_isDie) return;
        if (_isMove) return;

        if (context.started) _isInput = true;
        else if (context.canceled) _isInput = false;


        inputMoveMent = context.ReadValue<Vector3>();

        if (inputMoveMent != Vector3.zero)
        {
            _stateMachine.ChangeState(State.RUN);
        }
        else
        {
            _stateMachine.ChangeState(State.IDLE);
        }
    }

    private void MovePlayer()
    {
        if (_isDie)
        {
            inputMoveMent = Vector3.zero;
            return;
        }
        Vector3 moveDir = inputMoveMent * _moveSpeed * Time.deltaTime;

        if(moveDir != Vector3.zero)
        {
            Quaternion playerRotation = Quaternion.LookRotation(moveDir);
            rb.rotation = Quaternion.Lerp(playerRotation, rb.rotation, _rotSpeed * Time.deltaTime);
        }

        rb.MovePosition(rb.position + moveDir);
    }

    public void OnJump_Player(InputAction.CallbackContext context)
    { 
        //todo
    }

    public void OnDash_Player(InputAction.CallbackContext context)
    {
        if (_isDie) return;

        if(context.started)
        {
            _stateMachine.ChangeState(State.DASH);
        }
    }
    private void DashPlayer()
    { 
        if( _isDie)
        {
            inputMoveMent = Vector3.zero;
            return;
        }

        if (_isDash) return;

        StartCoroutine(AfterDash());
    }
    private IEnumerator AfterDash()
    {
        _isDash = true;
        _isMove = true;
        Vector3 dashDir;
        if (inputMoveMent == Vector3.zero)
        {
            dashDir = transform.forward;
        }
        else
        {
            dashDir = inputMoveMent;
        }
        rb.velocity = dashDir * _dashSpeed;

        yield return new WaitForSeconds(_dashTime);

        rb.velocity = Vector3.zero;

        yield return new WaitForSeconds(_dashCoolDown);

        if (inputMoveMent != Vector3.zero)
        {
            _stateMachine.ChangeState(State.RUN);
        }
        else
        {
            _stateMachine.ChangeState(State.IDLE);
        }
        _isMove = false;
        _isDash = false;
    }

    public void OnAttack_Player(InputAction.CallbackContext context)
    { 
    
    }

    public void OnGaurd_Player(InputAction.CallbackContext context)
    { 
        
    }

    public void OnAuxiliaryAttack_Player(InputAction.CallbackContext context)
    { 
    
    }

    public void OnInteraction_Player(InputAction.CallbackContext context)
    { 
    
    }

    public void OnPause_Player(InputAction.CallbackContext context)
    { 
    
    }


    private class BasePlayerState : BaseState
    {
        protected PlayerMove player;
        public BasePlayerState(PlayerMove player)
        {
            this.player = player;
        }
    }
    private class IdleState : BasePlayerState
    {
        public IdleState(PlayerMove player) : base(player) { }
        public override void Enter()
        {
            player.anim.SetBool(player.hashRun, false);
            //todo
        }
    }
    private class RunState : BasePlayerState
    { 
        public RunState(PlayerMove player) : base(player) { }
        public override void Enter()
        {
            player.anim.SetBool(player.hashRun, true);
        }
        public override void FixedUpdate()
        {
            player.MovePlayer();
        }
    }
    private class JumpState : BasePlayerState
    { 
        public JumpState(PlayerMove player) : base(player) { }
        public override void Enter()
        {
            player.anim.SetBool(player.hashJump, true);
            //todo
        }
    }
    private class DashState : BasePlayerState
    {
        public DashState(PlayerMove player) : base(player) { }
        public override void Enter()
        {
            player.anim.SetBool(player.hashRun, false);
            player.anim.SetTrigger(player.hashDash);
            player.DashPlayer();
        }
    }
    private class AttackState : BasePlayerState
    { 
        public AttackState(PlayerMove player) : base(player) { }
        public override void Enter()
        {
            player.anim.SetTrigger(player.hashAttack);
            //todo
        }
    }
    private class GaurdState : BasePlayerState
    { 
        public GaurdState(PlayerMove player) : base(player) { }
        public override void Enter()
        {
            player.anim.SetTrigger(player.hashGaurd);
            //todo
        }
    }
    private class DieState : BasePlayerState
    { 
        public DieState(PlayerMove player) : base(player) { }
        public override void Enter()
        {
            player.anim.SetTrigger(player.hashDie);
        }
    }
}
