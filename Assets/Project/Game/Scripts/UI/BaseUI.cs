using System;
using UnityEngine;

public class BaseUI : MonoBehaviour
{
    public event Action OnShow, OnHide;
    private GameObject _holder = null;
    public bool IsShow => _holder.activeSelf;

    protected virtual void Start()
    {
        if(transform.childCount > 0)
            _holder = transform.GetChild(0).gameObject;
    }

    public virtual void Show()
    {
        OnShow?.Invoke();
        if(_holder) _holder.SetActive(true);
    }
    public virtual void Hide()
    {
        OnHide?.Invoke();
        if(_holder) _holder.SetActive(false);
    }
}