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
    private StateMachine _stateMachine;

    private Rigidbody rb;
    private Animator anim;
    private NavMeshAgent nav;

    private Vector3 _defaultPos;
    private Quaternion _defaultRot;
    private Vector3 _playerLookAt;

    [SerializeField] private float _maxHp;
    [SerializeField] private float _hp;
    public float _damage;
    [SerializeField] private float _attakcRange;
    [SerializeField] private float _maxExp;
    [SerializeField] private float _minExp;
    private float _dropExp;

    private readonly int hashAttack = Animator.StringToHash("isAttack");
}
