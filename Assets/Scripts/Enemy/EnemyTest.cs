using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTest : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerAttack"))
        {
            Debug.Log("P");
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            Debug.Log("B");
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("SkillAttack"))
        {
            Debug.Log("S");
        }
    }
}
