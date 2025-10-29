using System.Collections;

using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
[DisallowMultipleComponent]
public class Music : MonoBehaviour
{
    #region Fields

    private const float MIN_CUTOFF_FREQUENCY = 500, MAX_CUTOFF_FREQUENCY = 22000;

    private AudioSource _audioSource;
    private AudioMixer _audioMixer;
    private int _musicTrackID;
    [SerializeField] private AudioClip[] _music;

    #endregion Fields

    #region Methods

    private void Awake()
    {
        _audioMixer = (AudioMixer)Resources.Load("Audio/AudioMixer");
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start() => _audioMixer.SetFloat("musicCutoffFrequency", MIN_CUTOFF_FREQUENCY);

    private void OnEnable()
    {
        ShuffleTheMusicTrack();
        _ = StartCoroutine(PlayMusic());
    }

    private IEnumerator PlayMusic()
    {
        while (true)
        {
            NextMusic();
            yield return new WaitForSeconds(_music[_musicTrackID - 1].length);
        }
    }

    /// <summary>
    /// Перемешать музыку
    /// </summary>
    [ContextMenu("ShuffleTheMusicTrack")]
    private void ShuffleTheMusicTrack()
    {
        _musicTrackID = 0;
        for (int i = 1; i < _music.Length; i++)
        {
            int k = i - 1;
            int id = Random.Range(k, _music.Length - k);
            (_music[k], _music[id]) = (_music[id], _music[k]);
        }
    }

    /// <summary>
    /// Плавная активация заглушки музыки (частота среза)
    /// </summary>
    /// <param name="duration">Длительность активации</param>
    /// <param name="on">заглушить?</param>
    /// <returns></returns>
    private IEnumerator ActivateCutoffFrequencyCoroutine(float duration, bool on)
    {
        //Инициализируем счётчиков прошедшего времени
        float elapsed = 0f;
        _ = _audioMixer.GetFloat("musicCutoffFrequency", out float startCutoffFrequency);
        float targetCutoffFrequency = on ? MIN_CUTOFF_FREQUENCY : MAX_CUTOFF_FREQUENCY;

        //Выполняем код до тех пор, пока не иссякнет время
        while (elapsed < duration)
        {
            _ = _audioMixer.SetFloat("musicCutoffFrequency", Mathf.Lerp(startCutoffFrequency, targetCutoffFrequency, elapsed / duration));
            elapsed += Time.deltaTime;
            yield return null;
        }
        _ = _audioMixer.SetFloat("musicCutoffFrequency", targetCutoffFrequency);
    }

    /// <summary>
    /// Плавное изменение громкости музыки
    /// </summary>
    /// <param name="duration">Длительность изменения</param>
    /// <param name="on">убавить?</param>
    /// <returns></returns>
    private IEnumerator ChangeVolumeCoroutine(float duration, bool on)
    {
        //Инициализируем счётчиков прошедшего времени
        float elapsed = 0f;

        float startVolume = _audioSource.volume;
        float targetVolume = on ? 0.5f : 1;
        //Выполняем код до тех пор, пока не иссякнет время
        while (elapsed < duration)
        {
            _audioSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        _audioSource.volume = targetVolume;
    }

    [ContextMenu("NextMusic")]
    public void NextMusic()
    {
        if (_musicTrackID >= _music.Length)
            ShuffleTheMusicTrack();
        if (_audioSource.isPlaying)
            _audioSource.Stop();
        _audioSource.PlayOneShot(_music[_musicTrackID]);
        _musicTrackID++;
    }

    /// <summary>
    /// Активация заглушки музыки (частота среза)
    /// </summary>
    /// <param name="on">заглушить?</param>
    public void ActivateCutoffFrequency(bool on)
    {
        _ = StartCoroutine(ActivateCutoffFrequencyCoroutine(2, on));
        _ = StartCoroutine(ChangeVolumeCoroutine(2, on));
    }

    #endregion Methods
}