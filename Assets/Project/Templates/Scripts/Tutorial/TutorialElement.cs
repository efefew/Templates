using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public abstract class TutorialElement<TUI> where TUI : TutorialUI
{
    protected enum ShowButtonType
    {
        NONE,
        /// <summary>
        /// Дублирование через Instantiate. Кнопка иногда может оказаться в неправильном месте
        /// </summary>
        CLONE,
        /// <summary>
        /// Перемещение заменой родителя. Наименее ресурсозатратная операция, но кнопка может оказаться в неправильном месте
        /// </summary>
        MOVE,
        /// <summary>
        /// Перемещение заменой родителя с учётом всяких LayoutGroup. Самая ресурсозатратная операция, но зато кнопка всегда в правильном месте
        /// </summary>
        FORCE_MOVE 
    }
    public enum EmotionType
    {
        DEFAULT,
        FUN,
        SAD,
        NONE
    }

    protected Tutorial<TUI> _tutorial;
    protected TutorialElement(Tutorial<TUI> tutorial)
    {
        _tutorial = tutorial;
    }
    protected void StartTutorial()
    {
        _tutorial.UI.BlockObj.SetActive(false);
        _tutorial.UI.MessageObj.SetActive(false);
        _tutorial.UI.TutorialObj.SetActive(true);
        EntryPoint.Mono.StartCoroutine(ITutorial());
    }
    protected void EndTutorial()
    {
        SaveManager.Save(SaveManager.TutorialData);
        _tutorial.UI.TutorialObj.SetActive(false);
    }
    protected virtual void OnDestroy()
    {
        _tutorial.UI.OnDestroyTutorialUI -= OnDestroy;
    }
    private void SetMessageListener(Button button, bool on)
    {
        if (on)
        {
            button.onClick.AddListener(_tutorial.NextStep);
        }
        else
        {
            button.onClick.RemoveListener(_tutorial.NextStep);
        }
    }

    private void Send(string message, EmotionType emotion = EmotionType.DEFAULT)
    {
        _tutorial.UI.MessageLabel.text = message;
    }
    protected abstract IEnumerator ITutorial();    
    protected WaitUntil WaitClickOnMarker(Action action, string message = null, EmotionType person = EmotionType.NONE)
    {
        _tutorial.UI.MessageObj.SetActive(true);
        Send(message, person);
        _tutorial.StepCompleted = false;
        return new WaitUntil(MarkerClickComplete(action));
    }
    protected WaitUntil WaitMessage(string message, EmotionType emotion = EmotionType.DEFAULT)
    {
        return WaitClick(_tutorial.UI.MessageButton, message, emotion);
    }
    protected WaitUntil WaitClick(Button button, string message = null, EmotionType person = EmotionType.NONE, bool block = true, ShowButtonType showButtonType = ShowButtonType.MOVE)
    {
        Button buttonClone = showButtonType is ShowButtonType.CLONE ? Object.Instantiate(button, _tutorial.UI.WaitButtonsContainer) : button;
        if (message != null)
        {
            if (!block)
            {
                _tutorial.UI.MessageButton.image.raycastTarget = false;
            }
            _tutorial.UI.MessageObj.SetActive(true);
            Send(message, person);
        }

        if (block)
        {
            _tutorial.UI.BlockObj.SetActive(true);
        }
        _tutorial.StepCompleted = false;
        
        SetMessageListener(buttonClone, true);
        return ShowButton(button, showButtonType, buttonClone);
    }

    private WaitUntil ShowButton(Button button, ShowButtonType showButtonType, Button buttonClone)
    {
        switch (showButtonType)
        {
            case ShowButtonType.CLONE:
                return new WaitUntil(ClickComplete(buttonClone));
            case ShowButtonType.MOVE:
            {
                int oldID = button.transform.GetSiblingIndex();
                Transform oldParent = button.transform.parent;
                button.transform.SetParent(_tutorial.UI.WaitButtonsContainer, worldPositionStays: true);
                return new WaitUntil(ClickComplete(buttonClone, oldParent, oldID));
            }
            case ShowButtonType.FORCE_MOVE:
            {
                int oldID = button.transform.GetSiblingIndex();
                Transform oldParent = button.transform.parent;
                button.GetComponent<RectTransform>().ForceChangeParentUI(_tutorial.UI.WaitButtonsContainer);
                return new WaitUntil(ClickComplete(buttonClone, oldParent, oldID));
            }
            case ShowButtonType.NONE:
                return new WaitUntil(ClickComplete(button));
            default:
                throw new ArgumentOutOfRangeException(nameof(showButtonType), showButtonType, null);
        }
    }
    private Func<bool> MarkerClickComplete(Action action)
    {
        return ()=>
        {
            if (_tutorial.StepCompleted)
            {
                action?.Invoke();
                _tutorial.UI.MessageObj.SetActive(false);
                _tutorial.UI.BlockObj.SetActive(false);
            }
            
            return _tutorial.StepCompleted;
        };
    }
    private Func<bool> ClickComplete(Button button, Transform oldParent, int oldID)
    {
        return ()=>
        {
            if (_tutorial.StepCompleted)
            {
                SetMessageListener(button, false);
                
                _tutorial.UI.MessageButton.image.raycastTarget = true;
                
                _tutorial.UI.MessageObj.SetActive(false);
                _tutorial.UI.BlockObj.SetActive(false);
                
                button.transform.SetParent(oldParent, worldPositionStays: true);
                button.transform.SetSiblingIndex(oldID);
            }
            
            return _tutorial.StepCompleted;
        };
    }
    private Func<bool> ClickComplete(Button button)
    {
        return ()=>
        {
            if (_tutorial.StepCompleted)
            {
                SetMessageListener(button, false);
                
                _tutorial.UI.MessageButton.image.raycastTarget = true;
                
                _tutorial.UI.MessageObj.SetActive(false);
                _tutorial.UI.BlockObj.SetActive(false);
                Object.Destroy(button.gameObject);
            }
            
            return _tutorial.StepCompleted;
        };
    }
}
