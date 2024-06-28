using Cinemachine;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    private CinemachineVirtualCamera _followCamera;
    private GameObject _player_CameraRoot;

    private void Awake()
    {
        _followCamera = GetComponent<CinemachineVirtualCamera>();
        _player_CameraRoot = GameObject.FindGameObjectWithTag("CameraRoot");
    }

    private void Start()
    {
        _followCamera.Follow = _player_CameraRoot.transform;
    }
}
