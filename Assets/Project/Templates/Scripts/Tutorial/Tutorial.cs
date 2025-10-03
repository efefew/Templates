using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class Tutorial<TUI> where TUI : TutorialUI
{
    protected Dictionary<Type, TutorialElement<TUI>> _tutorials;
    public TutorialConfig Config { get; }
    public TUI UI { get; }

    public bool StepCompleted { get; set; }
    public void NextStep() => StepCompleted = true;
    protected Tutorial(TUI ui)
    {
        SaveManager.LoadTutorial();
        UI = ui;
        Config = ui.TutorialConfig;
    }
    protected abstract void AddTutorials();
}