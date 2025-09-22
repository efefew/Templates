using System.Collections;
using UnityEngine.UI;

public class StartTutorialElement : TutorialElement
{
    public StartTutorialElement(Tutorial tutorial) : base(tutorial)
    {
        if(SaveManager.TutorialData.StartTutorialCompleted) return;
        StartTutorial();
    }
    protected override IEnumerator ITutorial()
    {
        yield return WaitMessage("WaitMessage with fun", EmotionType.Fun);
        yield return WaitMessage("WaitMessage");
        yield return WaitClick(_tutorial.UI.MessageButton, "button", block: false);
        SaveManager.TutorialData.StartTutorialCompleted = true;
        EndTutorial();
    }
}