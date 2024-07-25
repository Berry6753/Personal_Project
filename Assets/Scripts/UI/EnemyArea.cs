using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArea : MonoBehaviour
{
    private BoxCollider wallCol;

    private bool isIn;

    private void Awake()
    {
        wallCol = GetComponent<BoxCollider>();
    }

    private void OnEnable()
    {
        isIn = false;

        if (wallCol.isTrigger)
            ParticleManager.Instance.OnStopWallParticle();
        else
            ParticleManager.Instance.OnPlayWallParticle(transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isIn = !isIn;
            if (isIn)
            {
                PlayUIManager.Instance.TownNotice("��������");
            }
            else
            {
                PlayUIManager.Instance.TownNotice("��������");
            }
        }
    }
}
