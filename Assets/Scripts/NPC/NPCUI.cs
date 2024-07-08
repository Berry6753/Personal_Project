using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCUI : MonoBehaviour
{
    [SerializeField] private GameObject interactionUI;
    [SerializeField] private GameObject canvus;
    [SerializeField] private GameObject DiaLog;

    private bool isNear = false;
    private bool isDialog;

    private void OnEnable()
    {
       interactionUI.SetActive(false);
    }

    private void Update()
    {
        if (isNear)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                isDialog = true;
            }
        }

        if (!isDialog) return;

        canvus.SetActive(true);
        DiaLog.SetActive(true);

        if (!DiaLog.activeSelf)
        {
            isDialog = false;
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        { 
            interactionUI.SetActive(true);
            isNear = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            interactionUI.SetActive(false);
            isNear = false;
        }
    }
}
