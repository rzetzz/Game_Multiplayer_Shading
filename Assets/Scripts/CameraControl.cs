using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField]Quaternion oriRot;
    CinemachineVirtualCamera cam;
    PlayerControl[] player;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
        oriRot = transform.rotation;
    }

    private void Update() {
        
    }
}
