using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Button))]
public class Share : MonoBehaviour
{
    private Button _shareButton;
    [SerializeField] private string _subject  = "Play the AERO hockey!";
    [SerializeField] private string _text  = "Play the AERO hockey!";
    private void Start()
    {
        _shareButton = GetComponent<Button>();
        _shareButton.onClick.AddListener(ButtonShare);
    }

    private void OnDestroy()
    {
        _shareButton.onClick.RemoveAllListeners();
    }

    private void ButtonShare()
    {
        const string URL = "https://play.google.com/store/games";
        
        new NativeShare()
            .SetSubject(_subject).SetText(_text + URL).SetUrl(URL)
            .SetCallback((result, shareTarget) =>
                Debug.Log("Share result: " + result + ", selected app: " + shareTarget))
            .Share();
    }
}