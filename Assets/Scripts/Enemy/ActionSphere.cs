using UnityEngine;

public class ActionSphere : MonoBehaviour
{
    [SerializeField] private AlphaOmegaController ao;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        { 
            ao.ChangeMoveState("Trace");
        }           
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ao.ChangeMoveState("ComeBack");
        }
    }
}
