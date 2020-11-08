using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ViewBase : MonoBehaviour
{
    public GameObject ViewObject;

    protected virtual void OnShow(){}
    protected virtual void OnHide(){}
    protected virtual void OnInit(){}

    private void Awake()
    {
        OnInit();
    }

    public void Show()
    {
        OnShow();
        ViewObject.SetActive(true);
    }

    public void Hide()
    {
        OnHide();
        ViewObject.SetActive(false);
    }
}
