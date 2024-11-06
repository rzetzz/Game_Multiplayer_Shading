using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RotationToCamera : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameUI;
    void Start()
    {
        
    }

    private void Update() 
    {
        nameUI.text = GetComponentInParent<PlayerControl>().playerName.ToString();
    }

    
    private void LateUpdate() 
    {
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);    
    }
}
