using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class TutorialElement
{
    public enum EmotionType
    {
        Default,
        Fun,
        Sad,
        None
    }
    protected TutorialElement()
    {
    }
    protected void StartTutorial()
    {
        Tutorial.Instance.BlockObj.SetActive(false);
        Tutorial.Instance.MessageObj.SetActive(false);
        Tutorial.Instance.TutorialObj.SetActive(true);
        EntryPoint.Mono.StartCoroutine(ITutorial());
    }

    private static void SetMessageListener(Button button, bool on)
    {
        if (on)
        {
            button.onClick.AddListener(Tutorial.Instance.NextStep);
        }
        else
        {
            button.onClick.RemoveListener(Tutorial.Instance.NextStep);
        }
    }

    private static void Send(string message, EmotionType emotion = EmotionType.Default)
    {
        Tutorial.Instance.MessageLabel.text = message;
    }
    protected abstract IEnumerator ITutorial();    
    protected static void EndTutorial()
    {
        SaveManager.Save(SaveManager.TutorialData);
        Tutorial.Instance.TutorialObj.SetActive(false);
    }
    protected WaitUntil WaitMessage(string message, EmotionType emotion = EmotionType.Default)
    {
        return WaitClick(Tutorial.Instance.MessageButton, message, emotion);
    }
    protected WaitUntil WaitClick(Button button, string message = null, EmotionType emotion = EmotionType.None, bool block = true)
    {
        if (message != null)
        {
            if (!block)
            {
                Tutorial.Instance.MessageButton.image.raycastTarget = false;
            }
            Tutorial.Instance.MessageObj.SetActive(true);
            Send(message, emotion);
        }

        if (block)
        {
            Tutorial.Instance.BlockObj.SetActive(true);
        }
        Tutorial.Instance.StepCompleted = false;
        
        SetMessageListener(button, true);
        int oldID = button.transform.GetSiblingIndex();
        Transform oldParent = button.transform.parent;
        button.transform.parent = Tutorial.Instance.WaitButtonsContainer;
        
        return new WaitUntil(PredicateStepComplete(button, oldParent, oldID));
    }

    private Func<bool> PredicateStepComplete(Button button, Transform oldParent, int oldID)
    {
        return ()=>
        {
            if (Tutorial.Instance.StepCompleted)
            {
                SetMessageListener(button, false);
                
                Tutorial.Instance.MessageButton.image.raycastTarget = true;
                
                Tutorial.Instance.MessageObj.SetActive(false);
                Tutorial.Instance.BlockObj.SetActive(false);
                
                button.transform.parent = oldParent;
                button.transform.SetSiblingIndex(oldID);
            }
            
            return Tutorial.Instance.StepCompleted;
        };
    }
}
