using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UrlButton : MonoBehaviour
{
    [SerializeField] private string _url;
    private Button _button;

    private void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OpenURL);
    }
    private void OnDestroy()
    {
        _button.onClick.RemoveListener(OpenURL);
    }
    private void OpenURL() => Application.OpenURL(_url);
}
