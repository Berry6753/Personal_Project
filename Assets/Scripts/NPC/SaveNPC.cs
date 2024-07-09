using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveNPC : MonoBehaviour
{
    [SerializeField] private GameObject interactionUI;

    private Animator anim;
    private bool isNear = false;
    private readonly int hashNear = Animator.StringToHash("isNear");

    private void Awake()
    {
        anim = GetComponent<Animator>();        
    }

    private void OnEnable()
    {
        interactionUI.SetActive(false);
    }

    private void Update()
    {
        if (interactionUI.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                
            }
        }
    }

    private void ActiveUI()
    {
        interactionUI.SetActive(isNear);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isNear = true;
            anim.SetBool(hashNear, isNear);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isNear = false;
            anim.SetBool(hashNear, isNear);
        }
    }
}
