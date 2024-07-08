using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageInOut : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayUIManager.Instance.TownNotice("태초마을");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayUIManager.Instance.TownNotice("동물지역");
        }
    }
}
