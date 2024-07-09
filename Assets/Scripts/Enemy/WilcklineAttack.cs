using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WilcklineAttack : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        { 
            GameManager.Instance.PlayerHurt(GameManager.Instance.Wickline._damage);
            Debug.Log("위클라인이 플레이어 공격");
        }
    }
}
