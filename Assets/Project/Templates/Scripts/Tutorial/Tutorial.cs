using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial
{
    public Dictionary<Type, TutorialElement> Tutorials { get; } = new ();
    public TutorialUI UI { get; }

    public bool StepCompleted { get; set; }
    public void NextStep() => StepCompleted = true;
    public Tutorial(TutorialUI ui)
    {
        UI = ui;
        AddTutorials();
    }

    private void AddTutorials()
    {
        Tutorials.Add(typeof(StartTutorialElement), new StartTutorialElement(this));
    }
}