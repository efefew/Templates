#region

using System;
using UnityEngine;
using static PauseManager;

#endregion

public class Pause : MonoBehaviour, IPause
{
    private Vector3 _savedVelocity;
    private Vector3 _savedAngularVelocity;
    private Rigidbody _rb;
    private Rigidbody2D _rb2d;
    protected virtual void Awake()
    {
        ControlRigidbody();
        ControlRigidbody2D();
    }

    private void ControlRigidbody()
    {
        if (!TryGetComponent(out _rb)) return;
        OnPause += RigidbodyOnPause;
        OnResume += RigidbodyOnResume;
    }

    private void ControlRigidbody2D()
    {
        if (!TryGetComponent(out _rb2d)) return;
        OnPause += Rigidbody2DOnPause;
        OnResume += Rigidbody2DOnResume;
    }

    protected virtual void OnEnable()
    {
        if (PausedObjects.Contains(this)) return;
        PausedObjects.Add(this);
    }

    protected virtual void OnDisable()
    {
        PausedObjects.Remove(this);
    }

    protected virtual void OnDestroy()
    {
        PausedObjects.Remove(this);
        OnPause -= RigidbodyOnPause;
        OnResume -= RigidbodyOnResume;
        OnPause -= Rigidbody2DOnPause;
        OnResume -= Rigidbody2DOnResume;
    }

    public virtual void UpdatePause()
    {
    }

    public virtual void FixedUpdatePause()
    {
    }

    public virtual void LateUpdatePause()
    {
    }
    private void RigidbodyOnPause()
    {
        // Сохраняем текущую линейную и угловую скорость
        _savedVelocity = _rb.linearVelocity;
        _savedAngularVelocity = _rb.angularVelocity;

        // Отключаем физику полностью
        _rb.isKinematic = true;
        _rb.detectCollisions = false;
        _rb.useGravity = false;
    }
    private void RigidbodyOnResume()
    {
        // Сначала включаем физику
        _rb.isKinematic = false;
        _rb.detectCollisions = true;
        _rb.useGravity = true;
        
        // …затем восстанавливаем скорости
        _rb.linearVelocity = _savedVelocity;
        _rb.angularVelocity = _savedAngularVelocity;
    }
    private void Rigidbody2DOnPause()
    {
            _rb2d.simulated = false;
    }
    private void Rigidbody2DOnResume()
    {
            _rb2d.simulated = true;
    }
}