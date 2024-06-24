using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FordController : MonoBehaviour
{
    public enum State
    {
        IDLE,
        TRACE,
    }
    public State state = State.IDLE;
    private StateMachine _stateMachine;

    private Transform fordTr;
    private Transform playerTr;
    private List<GameObject> enemyList = new List<GameObject>();

    private float _speed;
    private float _followDistance = 0.1f;
    private float _lerpSpeed = 0.8f;

    private void Awake()
    {
        fordTr = GetComponent<Transform>();

        playerTr = GameObject.FindGameObjectWithTag("PlayerFordPos").transform;

        _stateMachine = gameObject.AddComponent<StateMachine>();
        
        _stateMachine.AddState(State.IDLE, new IdleState(this));
        _stateMachine.AddState(State.TRACE, new TraceState(this));
        _stateMachine.InitState(State.IDLE);
    }

    private void Start()
    {
        StartCoroutine(CoCheckFordState());
    }

    private IEnumerator CoCheckFordState()
    { 
        while (true)    // todo �÷��̾� ������� �̺�Ʈ ó���������� �޾ƿͼ� while�� �ȿ� ���� ����
        {
            yield return new WaitForSeconds(0.3f);

            if (true) // todo �÷��̾� ������� �̺�Ʈ ó���������� �޾ƿͼ� if�� �ȿ� ���� ����
            { 
                // todo
            }

            float distance = Vector3.Distance(fordTr.position, playerTr.position);

            if (distance < _followDistance)
            {
                _stateMachine.ChangeState(State.IDLE);
            }
            else
            {
                _stateMachine.ChangeState(State.TRACE);
            }
        }
    }

    private void ChasePlayer()
    {
        transform.position = Vector3.Lerp(fordTr.position, playerTr.position, _lerpSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(fordTr.rotation, playerTr.rotation, _lerpSpeed * Time.deltaTime);
    }

    public void OnAttack()
    { 
        //float[] distance = Vector3.Distance(fordTr.position, enemyList.)
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            enemyList.Add(other.gameObject);
        }
    }

    private class BaseFordState : BaseState
    {
        protected FordController ford;
        public BaseFordState(FordController ford)
        {
            this.ford = ford;
        }
    }
    private class IdleState : BaseFordState
    {
        public IdleState(FordController ford) : base(ford) { }
        public override void Enter()
        {

        }
    }
    private class TraceState : BaseFordState
    { 
        public TraceState(FordController ford) : base(ford) { }
        public override void Enter()
        {
            
        }
        public override void FixedUpdate()
        {
            ford.ChasePlayer();
        }
    }
}
