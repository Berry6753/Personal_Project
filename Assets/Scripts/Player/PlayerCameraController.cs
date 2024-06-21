using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] private Transform playerTr;
    [SerializeField] private Transform cameraTr;

    private Vector2 mouseDelta = Vector2.zero;

    private void Update()
    {
        LookAround();
    }

    private void LookAround()
    {       
        Vector3 camAngle = cameraTr.rotation.eulerAngles;
        float x = camAngle.x - mouseDelta.y;

        if (x < 180)
            x = Mathf.Clamp(x, -1f, 70f);
        else
            x = Mathf.Clamp(x, 335f, 361f);

        cameraTr.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);
    }

    public void OnRatation_Camera(InputAction.CallbackContext context)
    { 
        mouseDelta = context.ReadValue<Vector2>(); 
    }
}
