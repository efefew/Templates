using UnityEngine;
using UnityEngine.UI;

public class TabMenuUI : MonoBehaviour
{
    [SerializeField]
    private BaseUI[] _popups;
    [SerializeField]
    private Toggle[] _toggles;
    private void Start()
    {
        for (int id = 0; id < _toggles.Length; id++)
        {
            _toggles[id].onValueChanged.AddListener(HideAll);
            AddShow(id);
        }
    }

    private void AddShow(int id)
    {
        _toggles[id].onValueChanged.AddListener(on =>
        {
            if (on) _popups[id].Show();
        });
    }
    private void HideAll(bool value)
    {
        for (int id = 0; id < _popups.Length; id++) _popups[id].Hide();
    }
    private void OnDestroy()
    {
        for (int id = 0; id < _toggles.Length; id++)
            _toggles[id].onValueChanged.RemoveAllListeners();
    }
}
