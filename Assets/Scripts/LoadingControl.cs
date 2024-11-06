using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingControl : MonoBehaviour
{
    Animator anim;

    private void Start() {
        anim = GetComponent<Animator>();
    }
    public void StartLoading()
    {
        gameObject.SetActive(true);
    }
    public void EndLoading()
    {
        anim.SetTrigger("EndLoading");
    }
    public void DisableLoading()
    {
        gameObject.SetActive(false);
    }
}
