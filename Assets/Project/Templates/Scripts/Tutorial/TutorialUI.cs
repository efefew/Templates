using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    [field: SerializeField] public TutorialConfig TutorialConfig { get; private set; }
    [field: SerializeField] public GameObject MessageObj { get; private set; }
    [field: SerializeField] public GameObject BlockObj { get; private set; }
    [field: SerializeField] public GameObject TutorialObj { get; private set; }
    [field: SerializeField] public RectTransform WaitButtonsContainer { get; private set; }
    [field: SerializeField] public Button MessageButton { get; private set; }
    [field: SerializeField] public TMP_Text MessageLabel { get; private set; }
    
    [field: SerializeField] public Marker Marker { get; private set; }
    [field: SerializeField] public Finger Finger { get; private set; }
    public Action OnDestroyTutorialUI;

    protected void OnDestroy()
    {
        OnDestroyTutorialUI?.Invoke();
    }
}
