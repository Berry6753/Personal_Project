using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCameraController : MonoBehaviour
{
    [SerializeField] private bool x;
    [SerializeField] private bool y;
    [SerializeField] private bool z;
    private GameObject _target;

    private void Awake()
    {
        _target = GameObject.FindGameObjectWithTag("CameraRoot");
    }

    private void Update()
    {
        if (_target == null) return;

        transform.position = new Vector3(
            (x ? _target.transform.position.x : transform.position.x),
            (y ? _target.transform.position.y : transform.position.y),
            (z ? _target.transform.position.z : transform.position.z));
    }
}
