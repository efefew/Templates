using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public Dictionary<Type, TutorialElement> Tutorials { get; } = new ();
    [field: SerializeField] public GameObject MessageObj { get; private set; }
    [field: SerializeField] public GameObject BlockObj { get; private set; }
    [field: SerializeField] public GameObject TutorialObj { get; private set; }
    [field: SerializeField] public Transform WaitButtonsContainer { get; private set; }
    [field: SerializeField] public Button MessageButton { get; private set; }
    [field: SerializeField] public TMP_Text MessageLabel { get; private set; }
    public static Tutorial Instance { get; private set; }

    public bool StepCompleted { get; set; }
    public void NextStep() => StepCompleted = true;
    private void Start()
    {
        Instance ??= this;
        AddTutorials();
    }

    private void AddTutorials()
    {
        Tutorials.Add(typeof(StartTutorialElement), new StartTutorialElement(MessageButton));
    }
}