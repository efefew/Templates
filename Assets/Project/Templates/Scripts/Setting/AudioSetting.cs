using UnityEngine;
using UnityEngine.Audio;
public class AudioSetting : Setting
{
    [SerializeField]
    private AudioMixer _audioMixer;
    [SerializeField]
    private ToggleSwap _toggleMusic, _toggleSound;

    private const int MUTE = -80;

    protected override void Start()
    {
        base.Start();

        _audioMixer.SetFloat("MusicVolume", _setting.Music ? 0 : MUTE);
        _audioMixer.SetFloat("SoundsVolume", _setting.Sound ? 0 : MUTE);

        _toggleMusic.Toggle.isOn = _setting.Music;
        _toggleMusic.Swap(_setting.Music);
        _toggleMusic.Toggle.onValueChanged.AddListener(Music);

        _toggleSound.Toggle.isOn = _setting.Sound;
        _toggleSound.Swap(_setting.Sound);
        _toggleSound.Toggle.onValueChanged.AddListener(Sound);

    }
    public void Music(bool on)
    {
        _setting.Music = on;
        _audioMixer.SetFloat("MusicVolume", _setting.Music ? 0 : MUTE);
    }

    public void Sound(bool on)
    {
        _setting.Sound = on;
        _audioMixer.SetFloat("SoundsVolume", _setting.Sound ? 0 : MUTE);
    }
}
