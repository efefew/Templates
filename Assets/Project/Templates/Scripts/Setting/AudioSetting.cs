#region

using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

#endregion

public class AudioSetting : Setting
{
    private const int MIN = -80, MAX = 20;
    [SerializeField] private Slider _musicSlider, _soundsSlider;
    [SerializeField] private Toggle _musicToggle, _soundsToggle;
    [SerializeField] private AudioMixer _audioMixer;

    protected override void Start()
    {
        base.Start();
        Music(SaveManager.SettingData.Music);
        Sound(SaveManager.SettingData.Sound);
        SetMusicSlider();
        SetSoundSlider();
        SetMusicToggle();
        SetSoundToggle();
    }

    private void OnDestroy()
    {
        if (_musicSlider != null)
            _musicSlider.onValueChanged.RemoveListener(Music);
        if (_soundsSlider != null)
            _soundsSlider.onValueChanged.RemoveListener(Sound);
        if (_musicToggle != null)
            _musicToggle.onValueChanged.RemoveListener(Music);
        if (_soundsToggle != null)
            _soundsToggle.onValueChanged.RemoveListener(Sound);
    }

    private void SetMusicSlider()
    {
        if (_musicSlider == null) return;
        _musicSlider.value = SaveManager.SettingData.Music;
        _musicSlider.onValueChanged.AddListener(Music);
    }

    private void SetSoundSlider()
    {
        if (_soundsSlider == null) return;
        _soundsSlider.value = SaveManager.SettingData.Sound;
        _soundsSlider.onValueChanged.AddListener(Sound);
    }

    private void SetMusicToggle()
    {
        if (_musicToggle == null) return;
        _musicToggle.isOn = SaveManager.SettingData.Music > 0.5f;
        _musicToggle.onValueChanged.AddListener(Music);
    }

    private void SetSoundToggle()
    {
        if (_soundsToggle == null) return;
        _soundsToggle.isOn = SaveManager.SettingData.Sound > 0.5f;
        _soundsToggle.onValueChanged.AddListener(Sound);
    }

    private void Music(float value)
    {
        SaveManager.SettingData.Music = value;
        value = -value * MIN + MIN;
        _audioMixer.SetFloat("MusicVolume", value);
    }

    private void Sound(float value)
    {
        SaveManager.SettingData.Sound = value;

        value = -value * MIN + MIN;
        _audioMixer.SetFloat("SoundsVolume", value);
    }

    private void Music(bool on)
    {
        Music(on.ToInt());
    }

    private void Sound(bool on)
    {
        Sound(on.ToInt());
    }
}