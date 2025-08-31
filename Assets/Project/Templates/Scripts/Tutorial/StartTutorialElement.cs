using System.Collections;
using UnityEngine.UI;

public class StartTutorialElement : TutorialElement
{
    private Button _startTutorialButton;
    public StartTutorialElement(Button button) : base()
    {
        _startTutorialButton = button;
        if(SaveManager.TutorialData.StartTutorialCompleted) return;
        StartTutorial();
    }
    protected override IEnumerator ITutorial()
    {
        yield return WaitMessage("WaitMessage with fun", EmotionType.Fun);
        yield return WaitMessage("WaitMessage");
        yield return WaitClick(_startTutorialButton, "button", block: false);
        SaveManager.TutorialData.StartTutorialCompleted = true;
        EndTutorial();
    }
}