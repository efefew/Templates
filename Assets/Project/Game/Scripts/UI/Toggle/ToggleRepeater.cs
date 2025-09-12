using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class ToggleRepeater : MonoBehaviour
{
    private Toggle _toggle;
    [SerializeField] private Toggle _targetToggle;

    private void Awake()
    {
        _toggle = GetComponent<Toggle>();
        _targetToggle.onValueChanged.AddListener(RepeatToggle);
    }

    private void OnEnable()
    {
        _toggle.onValueChanged.RemoveListener(RepeatTargetToggle);
        _toggle.isOn = _targetToggle.isOn;
        _toggle.onValueChanged.AddListener(RepeatTargetToggle);
    }

    private void OnDestroy()
    {
        _toggle.onValueChanged.RemoveListener(RepeatTargetToggle);
        _targetToggle.onValueChanged.RemoveListener(RepeatToggle);
    }

    private void RepeatToggle(bool isOn)
    {
        _toggle.SetIsOnWithoutNotify(isOn);
    }
    private void RepeatTargetToggle(bool isOn)
    {
        _targetToggle.isOn = isOn;
    }
}
