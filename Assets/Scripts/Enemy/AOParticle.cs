using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOParticle : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.PlayerHurt(10.0f);
        }
    }  
}
