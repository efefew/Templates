using TMPro;

using UnityEngine;

public class DebugHUD : MonoBehaviour
{
    [SerializeField]
    private DebugMessageHUD _message;
    [SerializeField]
    private GameObject _hud;
    [SerializeField]
    private Transform _content;

    #region Unity Methods
    private void OnEnable() => Application.logMessageReceived += HandleLog;

    private void OnDisable() => Application.logMessageReceived -= HandleLog;
    private void OnDestroy() => Application.logMessageReceived -= HandleLog;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            _hud.SetActive(!_hud.activeInHierarchy);
        }
    }
    #endregion Unity Methods
    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        DebugMessageHUD message = Instantiate(_message, _content);
        TMP_Text label = message.Log;
        label.text = logString;
        switch (type)
        {
            case LogType.Error:
                label.color = Color.red;
                break;
            case LogType.Warning:
                label.color = Color.yellow;
                break;
            case LogType.Assert:
                label.color = Color.gray;
                break;
            case LogType.Exception:
                label.color = Color.red;
                break;
            case LogType.Log:
                label.color = Color.white;
                break;
            default:
                break;
        }
    }
}
