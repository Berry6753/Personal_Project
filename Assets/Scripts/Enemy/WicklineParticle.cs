using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WicklineParticle : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.PlayerHurt(20.0f);
        }
    }
}
