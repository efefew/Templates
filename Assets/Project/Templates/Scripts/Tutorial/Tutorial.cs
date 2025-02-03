using System.Linq;

using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    private bool _complete = false;

    [SerializeField]
    private TutorioalLevel[] _tutorials;
    [SerializeField]
    private Level[] _levels;

    [SerializeField]
    private GameObject _message;
    [SerializeField]
    private Button _messageButton;

    private bool _messageEnd;

    [SerializeField]
    private Text _messageLabel;
    public static Tutorial Instance { get; private set; }
    public bool Complete { get => _complete; set => _complete = value; }
    public GameObject Message => _message;
    public bool MessageEnd { get => _messageEnd; set => _messageEnd = value; }
    public Text MessageLabel => _messageLabel;

    private void EndMessage() => _messageEnd = true;

    public void StartTutorial(Level level)
    {
        _complete = false;
        if (_levels.Contains(level))
        {
            _tutorials[_levels.IndexOf(level)].StartTutorioal();
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        _messageButton.onClick.AddListener(EndMessage);
        _messageButton.onClick.AddListener(() => _message.SetActive(false));
    }
}