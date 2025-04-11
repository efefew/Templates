using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class DebugMessageHUD : MonoBehaviour
{
    [field: SerializeField]
    public TMP_Text Log { get; }
    [SerializeField]
    private Button _copyStackTrace;
    public void SetStackTrace(string stackTrace) => _copyStackTrace.onClick.AddListener(() => GUIUtility.systemCopyBuffer = stackTrace);
}
