using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCUI : MonoBehaviour
{
    [SerializeField] private GameObject interactionUI;
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject DiaLog;
    [SerializeField] private Button button;

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

        canvas.SetActive(true);
        DiaLog.SetActive(true);
    }

    public void OnExitInteraction()
    { 
        button.gameObject.SetActive(false);
        canvas.SetActive(false);
        DiaLog.SetActive(false);
        isDialog = false;
    }
    public void ReturnPlay()
    { 
        GameManager.Instance.PauseGame();
    }

    public void OnClick_AgreeLvPoint()
    {
        PlayUIManager.Instance.ActiveLvPointUI();
    }
    public void OnClick_AgreeQuest()
    {
        GameManager.Instance.SetIsTriggerWall();
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
