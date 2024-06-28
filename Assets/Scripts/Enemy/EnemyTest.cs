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
    }
}
