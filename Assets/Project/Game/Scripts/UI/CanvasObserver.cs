using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CanvasObserver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool PointerOverUI { get; private set; }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PointerOverUI = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PointerOverUI = false;
    }
}
