#region

using UnityEngine;
using UnityEngine.UI;

#endregion

[RequireComponent(typeof(Toggle))]
public class ToggleSwap : MonoBehaviour
{
    [SerializeField] private GameObject _on, _off;

    private Toggle _toggle;

    private void Awake()
    {
        _toggle = GetComponent<Toggle>();
        _toggle.onValueChanged.AddListener(Swap);
    }

    private void OnEnable()
    {
        Swap(_toggle.isOn);
    }

    private void OnDestroy()
    {
        _toggle.onValueChanged.RemoveListener(Swap);
    }

    private void Swap(bool on)
    {
        _on.SetActive(on);
        _off.SetActive(!on);
    }
}