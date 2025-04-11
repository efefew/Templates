using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioUI : MonoBehaviour
{
    protected AudioSource _audio;
    protected virtual void Awake()
    {
        _audio = GetComponent<AudioSource>();
        _audio.playOnAwake = false;
    }
}
