using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOSkill : MonoBehaviour
{
    private AlphaOmegaController ao;

    private void Awake()
    {
        ao = GetComponentInParent<AlphaOmegaController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.PlayerHurt(ao._damage * 2f);
            Debug.Log("AO가 플레이어 스킬공격");
        }
    }
}
