using UnityEngine;

public class AudioTouch : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    private void OnCollisionEnter(Collision other)
    {
        _audioSource.Play();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        _audioSource.Play();
    }
}
