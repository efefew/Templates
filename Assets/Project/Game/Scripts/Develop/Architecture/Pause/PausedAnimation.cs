#region

using System;
using System.Collections;
using UnityEngine;
using static PauseManager;

#endregion

public class PausedAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    private GameObject _objAnimator;
    [SerializeField] [Min(0)] private float _time;
    private Coroutine _coroutine;
    private float _oldSpeed;
    [SerializeField] private bool _isFreeTransform, _isDestroyedOnEnd;
    private Transform _parent;
    private Vector3 _originalPosition;
    private Quaternion _originalRotation;
    private void Awake()
    {
        _parent = transform.parent;
        _originalPosition = transform.localPosition;
        _originalRotation = transform.localRotation;
        _objAnimator = _animator.gameObject;
        PauseManager.OnPause += OnPause;
        PauseManager.OnResume += OnResume;
    }

    private void OnDestroy()
    {
        PauseManager.OnPause -= OnPause;
        PauseManager.OnResume -= OnResume;
    }

    private void OnResume()
    {
        _animator.speed = _oldSpeed;
    }
    private void OnPause()
    {
        _oldSpeed = _animator.speed;
        _animator.speed = 0;
    }
    public void Show()
    {
        if (_isFreeTransform)
        {
            Reset();
            transform.SetParent(_parent.parent);
        }
        if(_coroutine != null) StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(HideAnimationCoroutine());
    }

    private IEnumerator HideAnimationCoroutine()
    {
        _objAnimator.SetActive(false);
        yield return new WaitForFixedUpdate();
        _objAnimator.SetActive(true);
        yield return WaitForPausedSeconds(_time);
        _objAnimator.SetActive(false);
        if(_isDestroyedOnEnd)
            Destroy(gameObject);
        else
        {
                Reset();
        }
    }

    private void Reset()
    {
        if (!_parent) return;
        transform.SetParent(_parent);
        transform.localPosition = _originalPosition;
        transform.localRotation = _originalRotation;
    }
}