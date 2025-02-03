using System.Collections;

using UnityEngine;

public class TutorialLevel1 : TutorioalLevel
{
    public override void StartTutorioal()
    {
        base.StartTutorioal();
        StartCoroutine(ILevel());
    }
    protected override IEnumerator ILevel()
    {
        _tutorial.MessageLabel.text = "Пример текста в обучении";
        _tutorial.Message.SetActive(true);
        _tutorial.MessageEnd = false;
        yield return new WaitUntil(() => _tutorial.MessageEnd);
    }
}
