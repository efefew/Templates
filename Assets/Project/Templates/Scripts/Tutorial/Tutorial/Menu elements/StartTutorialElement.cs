using System.Collections;
using UnityEngine.UI;

public class StartTutorialElement : TutorialElement<TutorialMenuUI>
{
    public StartTutorialElement(MenuTutorial tutorial) : base(tutorial)
    {
        if(SaveManager.TutorialData.StartTutorialCompleted) return;
        StartTutorial();
    }

    // ReSharper disable once RedundantOverriddenMember
    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    protected override IEnumerator ITutorial()
    {
        yield return WaitMessage("WaitMessage with fun", EmotionType.FUN);
        yield return WaitMessage("WaitMessage");
        yield return WaitClick(_tutorial.UI.MessageButton, "button", block: false);
        SaveManager.TutorialData.StartTutorialCompleted = true;
        EndTutorial();
    }
}