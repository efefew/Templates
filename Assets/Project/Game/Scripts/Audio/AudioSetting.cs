#region

using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

#endregion

public class AudioSetting : MonoBehaviour
{
    private const int MIN = -80, MAX = 20;
    [SerializeField] private Toggle _musicToggle, _soundsToggle;

    [SerializeField] private AudioMixer _audioMixer;

    [SerializeField] 
    private Slider _musicSlider, _soundsSlider;
    [MenuItem("Tools/Audio/Mute")]
    public static void Mute()
    {
        SaveManager.SettingData.Music = 0;
        SaveManager.SettingData.Sound = 0;
    }
    [MenuItem("Tools/Audio/Unmute")]
    public static void Unmute()
    {
        SaveManager.SettingData.Music = 1;
        SaveManager.SettingData.Sound = 1;
    }
    private void Start()
    {
        Music(SaveManager.SettingData.Music);
        Sound(SaveManager.SettingData.Sound);
        SetMusicSlider();
        SetSoundSlider();
        SetMusicToggle();
        SetSoundToggle();
    }

    private void OnDestroy()
    {
        if (_musicSlider) _musicSlider.onValueChanged.RemoveListener(Music);
        if (_soundsSlider) _soundsSlider.onValueChanged.RemoveListener(Sound);
        if (_musicToggle) _musicToggle.onValueChanged.RemoveListener(Music);
        if (_soundsToggle) _soundsToggle.onValueChanged.RemoveListener(Sound);
    }

    private void SetMusicSlider()
    {
        if (!_musicSlider) return;
        _musicSlider.value = SaveManager.SettingData.Music;
        _musicSlider.onValueChanged.AddListener(Music);
    }

    private void SetSoundSlider()
    {
        if (!_soundsSlider) return;
        _soundsSlider.value = SaveManager.SettingData.Sound;
        _soundsSlider.onValueChanged.AddListener(Sound);
    }

    private void SetMusicToggle()
    {
        if (!_musicToggle) return;
        _musicToggle.isOn = SaveManager.SettingData.Music > 0.5f;
        _musicToggle.onValueChanged.AddListener(Music);
    }

    private void SetSoundToggle()
    {
        if (!_soundsToggle) return;
        _soundsToggle.isOn = SaveManager.SettingData.Sound > 0.5f;
        _soundsToggle.onValueChanged.AddListener(Sound);
    }
    private void Music(float value)
    {
        SaveManager.SettingData.Music = value;
        value = -value * MIN + MIN;
        _audioMixer.SetFloat("MusicVolume", value);
        SaveManager.Save(SaveManager.SettingData);
    }

    private void Sound(float value)
    {
        SaveManager.SettingData.Sound = value;

        value = -value * MIN + MIN;
        _audioMixer.SetFloat("SoundVolume", value);
        SaveManager.Save(SaveManager.SettingData);
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