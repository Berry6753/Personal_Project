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
        JUMP,
        DASH,
        ATTACK,
        GAURD,
        DIE
    }
    public State state = State.IDLE;

    private CharacterController cc;   
    private Animator anim;
    private GameObject _mainCamera;

    private StateMachine _stateMachine;
    private PlayerInfo _playerInfo;

    private readonly int hashRun = Animator.StringToHash("isRun");
    private readonly int hashDash = Animator.StringToHash("isDash");
    private readonly int hashAttack = Animator.StringToHash("isAttack");
    private readonly int hashGaurd = Animator.StringToHash("isGaurd");
    private readonly int hashDie = Animator.StringToHash("isDie");
    private readonly int hashAttackCount = Animator.StringToHash("AttackCount");

    private Vector3 dir;
    private Vector3 inputMoveMent = Vector3.zero;
    private Vector2 inputRotation = Vector2.zero;

    [SerializeField] private int _playerLayer;
    [SerializeField] private int _dashLayer;

    private float _moveSpeed = 5.0f;
    private float _rotSpeed = 10.0f;
    private float _dashSpeed = 20.0f;
    private float _jumpSpeed = 20.0f;

    private float _jumpTime = 2.0f;
    private float _dashTime = 0.5f;
    private float _dashCoolDown = 0.2f;
    private float _auxiliary;
    private float _auxiliaryTimer = 0f;
    public float _gaurd;

    [SerializeField] private GameObject chinemachineTarget;
    private float _chinemachineTargetYaw;
    private float _chinemachineTargetPitch;

    [SerializeField] private BoxCollider _attackCollider;
    [SerializeField] private GameObject _fourthAttackEffect;
    [SerializeField] private BoxCollider _fourthAttackCollider;

    private float _topClamp = 70.0f;
    private float _bottomClamp = -30.0f;

    private int _attackCount = 0;

    private bool _isMove = false;
    private bool _isDash = false;
    private bool _isJump = false;
    private bool _isAttack = false;
    private bool _isGaurd = false;
    private bool _isDie = false;
    private bool _isInput = false;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        if (_mainCamera == null)
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

        _stateMachine = gameObject.AddComponent<StateMachine>();

        _stateMachine.AddState(State.IDLE, new IdleState(this));
        _stateMachine.AddState(State.RUN, new RunState(this));
        _stateMachine.AddState(State.JUMP, new JumpState(this));
        _stateMachine.AddState(State.DASH, new DashState(this));
        _stateMachine.AddState(State.ATTACK, new AttackState(this));
        _stateMachine.AddState(State.GAURD, new GaurdState(this));
        _stateMachine.AddState(State.DIE, new DieState(this));
        _stateMachine.InitState(State.IDLE);

        _playerInfo = GetComponent<PlayerInfo>();
        _playerInfo.onDie += OnPlayerDie;
    }

    private void Update()
    {
        if (_auxiliary != 0)
        {
            _auxiliaryTimer += Time.deltaTime;
            if (_auxiliaryTimer > 0.1f)
            {
                GameManager.Instance.ShootBullet();
                _auxiliaryTimer = 0;
            }
        }

        LookPlayer();

        if (!cc.isGrounded)
        {
            dir.y += Physics.gravity.y * Time.deltaTime;
            cc.Move(dir * Time.deltaTime);
        }
        else
        {
            dir.y = Physics.gravity.y;
            cc.Move(dir * Time.deltaTime);
            _isJump = false;
        }
        Debug.DrawRay(transform.position, Vector3.down, Color.red, 0.1f);
    }

    public void OnMove_Player(InputAction.CallbackContext context)
    {
        if (Cursor.lockState == CursorLockMode.None) return;
        if (_isDie) return;

        inputMoveMent = context.ReadValue<Vector3>();

        if (_isMove) return;
        if (_isAttack) return;
        if (_isGaurd) return;
        //if(_isJump) return;

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

        Vector3 moveDir = _mainCamera.transform.forward * inputMoveMent.z + _mainCamera.transform.right * inputMoveMent.x;
        moveDir.y = 0;
        moveDir.Normalize();
        moveDir *= _moveSpeed * Time.deltaTime;

        if (moveDir != Vector3.zero)
        {
            Quaternion playerRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Lerp(playerRotation, transform.rotation, _rotSpeed * Time.deltaTime);
        }

        cc.Move(moveDir);
    }

    public void OnLook_Player(InputAction.CallbackContext context)
    {
        if (Cursor.lockState == CursorLockMode.None)
        {
            inputRotation = Vector2.zero;
            return;
        }
        inputRotation = context.ReadValue<Vector2>();
    }
    private void LookPlayer()
    {
        if (_mainCamera == null) return;

        _chinemachineTargetYaw += inputRotation.x;
        _chinemachineTargetPitch -= inputRotation.y;

        _chinemachineTargetYaw = ClampAngle(_chinemachineTargetYaw, float.MinValue, float.MaxValue);
        _chinemachineTargetPitch = ClampAngle(_chinemachineTargetPitch, _bottomClamp, _topClamp);

        chinemachineTarget.transform.rotation = Quaternion.Euler(_chinemachineTargetPitch, _chinemachineTargetYaw, 0.0f);
    }
    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360f) angle += 360f;
        if (angle > 360f) angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }

    public void OnJump_Player(InputAction.CallbackContext context)
    {
        if (Cursor.lockState == CursorLockMode.None) return;
        if (_isDie) return;
        if (_isAttack) return;
        if (_isGaurd) return;
        if (_isJump) return;

        if (context.started)
        {
            _stateMachine.ChangeState(State.JUMP);
        }
    }
    private void JumpPlayer()
    {
        StartCoroutine(CoJumpPlayer());
    }
    private IEnumerator CoJumpPlayer()
    {
        _isJump = true;

        float endTime = Time.time + _jumpTime;
        Vector3 moveDir = _mainCamera.transform.forward * inputMoveMent.z + _mainCamera.transform.right * inputMoveMent.x;
        moveDir.y = 0;
        moveDir.Normalize();
        while (Time.time < endTime)
        {
            Vector3 jumpDir = (transform.up * _jumpSpeed + moveDir * _moveSpeed) * Time.deltaTime;
            cc.Move(jumpDir);
            yield return null;
        }
    }

    public void OnDash_Player(InputAction.CallbackContext context)
    {
        if (Cursor.lockState == CursorLockMode.None) return;
        if (_isDie) return;
        if (_isMove) return;
        if (_isAttack) return;
        if (_isGaurd) return;

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

        gameObject.layer = _dashLayer;

        float endTime = Time.time + _dashTime;
        while (Time.time < endTime)
        {
            cc.Move(transform.forward * _dashSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(_dashCoolDown);

        gameObject.layer = _playerLayer;

        ChangeStateAfterMove();
        _isMove = false;
        _isDash = false;
    }
    public void OnAttack_Player(InputAction.CallbackContext context)
    {
        if (Cursor.lockState == CursorLockMode.None) return;
        if (_isDie) return;
        if (_isMove) return;
        if (_isGaurd) return;
        if (_isJump) return;

        if (context.started)
        {
            _isAttack = true;
            _stateMachine.ChangeState(State.ATTACK);
        }
    }

    public void OnGaurd_Player(InputAction.CallbackContext context)
    {
        if (Cursor.lockState == CursorLockMode.None) return;
        if (_isDie) return;
        if (_isMove) return;
        if (_isAttack) return;

        _gaurd = context.ReadValue<float>();
        if (_gaurd != 0)
            _stateMachine.ChangeState(State.GAURD);
        else
            ChangeStateAfterMove();
    }
    private void OnGaurd()
    {
        inputMoveMent = Vector3.zero;
        _isGaurd = true;
    }

    public void OnAuxiliaryAttack_Player(InputAction.CallbackContext context)
    {
        if (Cursor.lockState == CursorLockMode.None) return;
        if (_isDie) return;

        _auxiliary = context.ReadValue<float>();
    }

    public void OnPause_Player(InputAction.CallbackContext context)
    {
        GameManager.Instance.PauseGame();
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

    private void OnPlayerDie(PlayerInfo playerInfo)
    {
        if (state == State.DIE) return;

        _stateMachine.ChangeState(State.DIE);
        _isDie = true;
    }

    private void EnableAttackCollider()
    { 
        _attackCollider.enabled = true;
    }
    private void DisAbleAttackCollider()
    {
        _attackCollider.enabled = false;
    }
    private void ActiveFourthAttackEffect()
    {
        _fourthAttackEffect.SetActive(true);
    }
    private void UnActiveFourthAttackEffect()
    {
        _fourthAttackEffect.SetActive(false);
    }
    private void EnableFourthAttackCollider()
    {
        _fourthAttackCollider.enabled = true;
    }
    private void DisableFourthAttackCollider()
    { 
        _fourthAttackCollider.enabled = false;
    }

    private void OnApplicationFocus(bool focus)
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnApplicationPause(bool pause)
    {
        Cursor.lockState = CursorLockMode.None;
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
    private class JumpState : BasePlayerState
    {
        public JumpState(PlayerMove player) : base(player) { }
        float jump;
        public override void Enter()
        {
            player.JumpPlayer();
            jump = Time.time + player._jumpTime;

            player.state = State.JUMP;
        }
        public override void Update()
        {
            if (player.cc.isGrounded && Time.time > jump)
            {
                player.ChangeStateAfterMove();
            }
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
            player.anim.SetBool(player.hashRun, false);
            player.anim.SetBool(player.hashGaurd, true);
            player.OnGaurd();
            //todo

            player.state = State.GAURD;
        }
        public override void Exit()
        {
            player.anim.SetBool(player.hashGaurd, false);
            player._isGaurd = false;
        }
    }
    private class DieState : BasePlayerState
    { 
        public DieState(PlayerMove player) : base(player) { }
        public override void Enter()
        {
            player.anim.SetTrigger(player.hashDie);
            Debug.Log("asdfasfesaf");

            player.state = State.DIE;
        }
    }
}
