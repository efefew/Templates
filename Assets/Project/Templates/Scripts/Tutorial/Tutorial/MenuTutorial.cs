public class MenuTutorial : Tutorial <TutorialMenuUI>
{
    public MenuTutorial(TutorialMenuUI ui) : base(ui)
    {
        AddTutorials();
    }

    protected sealed override void AddTutorials()
    {
        _tutorials.Add(typeof(StartTutorialElement), new StartTutorialElement(this));
    }
}