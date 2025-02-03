using UnityEngine;

[RequireComponent(typeof(LanguageSetting))]
[RequireComponent(typeof(GraphicSetting))]
[RequireComponent(typeof(ControlSetting))]
[RequireComponent(typeof(AudioSetting))]
[RequireComponent(typeof(GameSetting))]
public class SettingManager : MonoBehaviour
{
    public LanguageSetting LanguageSetting { get; private set; }
    public GraphicSetting GraphicSetting { get; private set; }
    public ControlSetting ControlSetting { get; private set; }
    public AudioSetting AudioSetting { get; private set; }
    public GameSetting GameSetting { get; private set; }

    private void Awake()
    {
        //DontDestroyOnLoad(this);
        LanguageSetting = GetComponent<LanguageSetting>();
        GraphicSetting = GetComponent<GraphicSetting>();
        ControlSetting = GetComponent<ControlSetting>();
        AudioSetting = GetComponent<AudioSetting>();
        GameSetting = GetComponent<GameSetting>();
    }
}
