using UnityEngine;

[RequireComponent(typeof(Animation))]
public class AnimationActivator : MonoBehaviour
{
    [SerializeField] private Animation _animation;

    private void Awake()
    {
        _animation = GetComponent<Animation>();
    }
    public void Play(AnimationClip clip)
    {
        _animation.Play(clip.name);
    }
    /*public void Play(string animationName)
    {
        _animation.Play(animationName);
    }*/
}
