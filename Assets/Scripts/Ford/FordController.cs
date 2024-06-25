using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class FordController : MonoBehaviour
{
    public enum State
    {
        IDLE,
        TRACE,
    }
    public State state = State.IDLE;
    private StateMachine _stateMachine;

    private IObjectPool<BulletMove> _bulletPool;

    private Transform fordTr;
    private Transform playerTr;
    private List<GameObject> enemyList = new List<GameObject>();

    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private GameObject _shootPoint;

    private float _speed;
    private float _followDistance = 0.1f;
    private float _lerpSpeed = 0.8f;
    private float _fireTime = 0.5f;
    public float fordDmg;

    private bool _isAttack = false;

    private void Awake()
    {
        fordTr = GetComponent<Transform>();

        playerTr = GameObject.FindGameObjectWithTag("PlayerFordPos").transform;

        _stateMachine = gameObject.AddComponent<StateMachine>();
        
        _stateMachine.AddState(State.IDLE, new IdleState(this));
        _stateMachine.AddState(State.TRACE, new TraceState(this));
        _stateMachine.InitState(State.IDLE);

        _bulletPool = new ObjectPool<BulletMove>(CreateBullet, OnGetBullet, OnReleaseBullet, OnDestroyBullet);
    }

    private void Start()
    {
        StartCoroutine(CoCheckFordState());
    }

    private IEnumerator CoCheckFordState()
    { 
        while (true)    // todo 플레이어 사망정보 이벤트 처리형식으로 받아와서 while문 안에 조건 변경
        {
            yield return new WaitForSeconds(0.3f);

            if (true) // todo 플레이어 사망정보 이벤트 처리형식으로 받아와서 if문 안에 조건 변경
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
    }
    private void RotateFord()
    {
        if (enemyList.Count > 0 && _isAttack)
        {
            transform.LookAt(enemyList[0].transform.position);
        }
        else
        {
            transform.LookAt(null);
            transform.rotation = Quaternion.Lerp(fordTr.rotation, playerTr.rotation, _lerpSpeed * Time.deltaTime);
        }
    }

    private void CheckDistanceofEnemy()
    {
        if (enemyList.Count <= 1) return;

        enemyList.Sort((a, b) =>
        {
            float distanceA = Vector3.Distance(transform.position, a.transform.position);
            float distanceB = Vector3.Distance(transform.position, b.transform.position);

            return distanceA.CompareTo(distanceB);
        });
    }

    public void OnAttack()
    {
        if (enemyList.Count <= 0) return;

        _isAttack = true;
        var bullet = _bulletPool.Get();
        bullet.Shoot();
        bullet.transform.position = _shootPoint.transform.position;
        bullet.transform.rotation = _shootPoint.transform.rotation;
    }

    private BulletMove CreateBullet()
    {   
        BulletMove bullet = Instantiate(_bulletPrefab, _shootPoint.transform.position, _shootPoint.transform.rotation, _shootPoint.transform).GetComponent<BulletMove>();
        bullet.SetManagedPool(_bulletPool);
        return bullet;
    }
    private void OnGetBullet(BulletMove bullet)
    { 
        bullet.gameObject.SetActive(true);
    }
    private void OnReleaseBullet(BulletMove bullet)
    { 
        bullet.gameObject.SetActive(false);
    }
    private void OnDestroyBullet(BulletMove bullet)
    {
        Destroy(bullet.gameObject);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            enemyList.Add(other.gameObject);
            CheckDistanceofEnemy();
            Debug.Log("b");
        }
    }
    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Enemy"))
    //    {
    //        enemyList.Add(other.gameObject);
    //        CheckDistanceofEnemy();
    //        Debug.Log("a");
    //    }
    //}
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            enemyList.Remove(other.gameObject);
            CheckDistanceofEnemy();
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
        public override void FixedUpdate()
        {
            ford.RotateFord();
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
            ford.RotateFord();
        }
    }
}
