using System;
using UnityEngine;
using UnityEngine.UI;

public class PopupUI : BaseUI
{
    [SerializeField]
    private BaseUI[] _popups;
    [SerializeField]
    private Button[] _buttons;

    protected override void Start()
    {
        base.Start();
        for (int id = 0; id < _buttons.Length; id++)
        {
            _buttons[id].onClick.AddListener(Hide);
            _buttons[id].onClick.AddListener(_popups[id].Show);
        }
    }

    private void OnDestroy()
    {
        for (int id = 0; id < _buttons.Length; id++)
            _buttons[id].onClick.RemoveAllListeners();
    }
}
