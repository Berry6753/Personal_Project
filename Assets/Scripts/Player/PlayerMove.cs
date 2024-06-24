using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public enum State
    {
        IDLE,
        RUN,
        //JUMP,
        DASH,
        ATTACK,
        GAURD,
        DIE
    }
    public State state = State.IDLE;

    private Rigidbody rb;
    private Animator anim;

    private StateMachine _stateMachine;

    private readonly int hashRun = Animator.StringToHash("isRun");
    //private readonly int hashJump = Animator.StringToHash("isJump");
    private readonly int hashDash = Animator.StringToHash("isDash");
    private readonly int hashAttack = Animator.StringToHash("isAttack");
    private readonly int hashGaurd = Animator.StringToHash("isGaurd");
    private readonly int hashDie = Animator.StringToHash("isDie");
    private readonly int hashAttackCount = Animator.StringToHash("AttackCount");

    private Vector3 inputMoveMent = Vector3.zero;

    private float _moveSpeed = 5.0f;
    private float _rotSpeed = 10.0f;
    private float _dashSpeed = 20.0f;
    private float _jumpSpeed;

    private float _dashTime = 0.5f;
    private float _dashCoolDown = 0.2f;

    //private int _jumpCount;
    private int _attackCount = 0;

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
        //_stateMachine.AddState(State.JUMP, new JumpState(this));
        _stateMachine.AddState(State.DASH, new DashState(this));
        _stateMachine.AddState(State.ATTACK, new AttackState(this));
        _stateMachine.AddState(State.GAURD, new GaurdState(this));
        _stateMachine.AddState(State.DIE, new DieState(this));
        _stateMachine.InitState(State.IDLE);
    }

    public void OnMove_Player(InputAction.CallbackContext context)
    {
        if (_isDie) return;

        inputMoveMent = context.ReadValue<Vector3>();

        if (_isMove) return;
        if(_isAttack) return;

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

        if (moveDir != Vector3.zero)
        {
            Quaternion playerRotation = Quaternion.LookRotation(moveDir);
            rb.rotation = Quaternion.Lerp(playerRotation, rb.rotation, _rotSpeed * Time.deltaTime);
        }

        rb.MovePosition(rb.position + moveDir);
    }

    //public void OnJump_Player(InputAction.CallbackContext context)
    //{ 
    //    //todo
    //}

    public void OnDash_Player(InputAction.CallbackContext context)
    {
        if (_isDie) return;
        if (_isMove) return;
        if (_isAttack) return;

        if (context.started)
        {
            _stateMachine.ChangeState(State.DASH);
        }
    }
    private void DashPlayer()
    {
        if (_isDie)
        {
            inputMoveMent = Vector3.zero;
            return;
        }

        if (_isDash) return;

        StartCoroutine(CoAfterDash());
    }
    private IEnumerator CoAfterDash()
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

        ChangeStateAfterMove();
        _isMove = false;
        _isDash = false;
    }
    public void OnAttack_Player(InputAction.CallbackContext context)
    {
        if (_isDie) return;
        if (_isMove) return;

        if (context.started)
        {
            _isAttack = true;
            _stateMachine.ChangeState(State.ATTACK);
        }
    }

    public void OnGaurd_Player(InputAction.CallbackContext context)
    {
        if (_isDie) return;
        if (_isMove) return;
    }

    public void OnAuxiliaryAttack_Player(InputAction.CallbackContext context)
    {
        if (_isDie) return;
    }

    public void OnInteraction_Player(InputAction.CallbackContext context)
    { 
    
    }

    public void OnPause_Player(InputAction.CallbackContext context)
    { 
    
    }

    public void ChangeStateAfterMove()
    {
        if (CheckInput())
            _stateMachine.ChangeState(State.RUN);
        else
            _stateMachine.ChangeState(State.IDLE);
    }
    private bool CheckInput()
    {
        if (inputMoveMent == Vector3.zero)
            _isInput = false;
        else
            _isInput = true;
        return _isInput;
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

            player.state = State.IDLE;
        }
    }
    private class RunState : BasePlayerState
    { 
        public RunState(PlayerMove player) : base(player) { }
        public override void Enter()
        {
            player.anim.SetBool(player.hashRun, true);

            player.state = State.RUN;
        }
        public override void FixedUpdate()
        {
            player.MovePlayer();
        }
    }
    //private class JumpState : BasePlayerState
    //{ 
    //    public JumpState(PlayerMove player) : base(player) { }
    //    public override void Enter()
    //    {
    //        player.anim.SetBool(player.hashJump, true);
    //        //todo
    //    }
    //}
    private class DashState : BasePlayerState
    {
        public DashState(PlayerMove player) : base(player) { }
        public override void Enter()
        {
            player.anim.SetBool(player.hashRun, false);
            player.anim.SetTrigger(player.hashDash);
            player.DashPlayer();

            player.state = State.DASH;
        }
        public override void Exit()
        {
            player.anim.ResetTrigger(player.hashDash);
        }
    }
    private class AttackState : BasePlayerState
    { 
        public AttackState(PlayerMove player) : base(player) { }
        public override void Enter()
        {
            player.anim.SetTrigger(player.hashAttack);
            player.anim.SetInteger(player.hashAttackCount, player._attackCount);

            player.state = State.ATTACK;
        }
        public override void Exit()
        {
            player.anim.ResetTrigger(player.hashAttack);
            player._isAttack = false;
        }
    }
    private class GaurdState : BasePlayerState
    { 
        public GaurdState(PlayerMove player) : base(player) { }
        public override void Enter()
        {
            player.anim.SetTrigger(player.hashGaurd);
            //todo

            player.state = State.GAURD;
        }
        public override void Exit()
        {
            player.anim.ResetTrigger(player.hashGaurd);
        }
    }
    private class DieState : BasePlayerState
    { 
        public DieState(PlayerMove player) : base(player) { }
        public override void Enter()
        {
            player.anim.SetTrigger(player.hashDie);

            player.state = State.DIE;
        }
    }
}
