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

    private StateMachine _stateMachine;

    private readonly int hashRun = Animator.StringToHash("isRun");
    private readonly int hashJump = Animator.StringToHash("isJump");
    private readonly int hashAttack = Animator.StringToHash("isAttack");
    private readonly int hashGaurd = Animator.StringToHash("isGaurd");
    private readonly int hashDie = Animator.StringToHash("isDie");

    private Vector3 inputMoveMent = Vector3.zero;

    private float _moveSpeed;
    private float _dashSpeed;
    private float _jumpSpeed;

    private int _jumpCount;
    private int _attackCount;

    private bool _isDash;
    private bool _isJump;
    private bool _isAttack;
    private bool _isDie;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        _stateMachine = gameObject.AddComponent<StateMachine>();

        _stateMachine.AddState(State.IDLE, new IdleState(this));
        _stateMachine.AddState(State.RUN, new RunState(this));
        _stateMachine.AddState(State.JUMP, new JumpState(this));
        _stateMachine.AddState(State.ATTACK, new AttackState(this));
        _stateMachine.AddState(State.GAURD, new GaurdState(this));
        _stateMachine.AddState(State.DIE, new DieState(this));
    }

    public void OnMove_Player(InputAction.CallbackContext context)
    { 
        inputMoveMent = context.ReadValue<Vector3>();
    }

    public void OnJump_Player(InputAction.CallbackContext context)
    { 
        //todo
    }

    public void OnDash_Player(InputAction.CallbackContext context)
    { 
    
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
            //todo
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
