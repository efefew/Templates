using System;

using UnityEngine;

[RequireComponent(typeof(Camera))]
public class Camera3DFromAbove : BootstrapElement
{
    public static event Action OnUpdateMove, OnUpdateRotate;

    private const float MIN_DELTA = 0.1f;
    private const int CIRCLE_ANGLE = 360;
    private Controller _controller;
    [SerializeField]
    private Transform _target;

    [SerializeField]
    private float _speedMove = 3, _speedRotate = 1, _speedZoom = 1, _zoomMin = 100, _zoomMax = 200, _lerpMove = 0.01f, _lerpRotate = 0.01f, _distanceZ = 5, _angleX = 30;

    private float _targetAngleY = 0;
    private Transform _trCamera;

    #region Unity Methods
    public override void StartBootstrap()
    {
        _controller = Controller.Instance;
        _controller.CameraController.OnMove += Move;
        _controller.CameraController.OnRotate += Rotate;
        _controller.CameraController.OnZoom += Zoom;
        _controller.CameraController.OnReset += ResetCamera;
        _trCamera = transform;
        _ = _trCamera.SetAngleX(_angleX);
        _target.position = _trCamera.position;
        _target.eulerAngles = _trCamera.eulerAngles;
    }

    private void Update() => _controller.CameraController.Control();

    private void LateUpdate() => CameraUpdate();

    #endregion Unity Methods

    private void CameraUpdate()
    {
        Vector3 angle = new(0, _targetAngleY, 0);
        _target.rotation = Quaternion.Slerp(_target.rotation, Quaternion.Euler(angle), _lerpRotate);
        _ = _trCamera.SetAngleX(0);
        _trCamera.position += _trCamera.forward * _distanceZ;
        _trCamera.position = Vector3.Lerp(_trCamera.position, _target.position, _lerpMove);
        _trCamera.rotation = _target.rotation;
        _trCamera.position -= _trCamera.forward * _distanceZ;
        _ = _trCamera.SetAngleX(_angleX);
    }
    private void Move(Vector2 position)
    {
        if (position == Vector2.zero)
        {
            return;
        }

        OnUpdateMove?.Invoke();

        float oldY = _target.position.y;
        _target.position -= _target.right * position.x * _speedMove;
        _target.position -= _target.forward * position.y * _speedMove;
        _ = _target.SetPositionY(oldY);
    }
    private void Rotate(float scaleRotation)
    {
        if (scaleRotation == 0)
        {
            return;
        }

        OnUpdateRotate?.Invoke();
        _targetAngleY += scaleRotation;
        if (_targetAngleY is < (-CIRCLE_ANGLE) or > CIRCLE_ANGLE)
        {
            _targetAngleY = 0;
        }
    }

    private void Zoom(float scaleZoom)
    {
        _target.position += Vector3.up * scaleZoom * _speedZoom;
        if (_trCamera.position.y > _zoomMax + MIN_DELTA)
        {
            _ = _target.SetPositionY(_zoomMax);
        }

        if (_trCamera.position.y < _zoomMin - MIN_DELTA)
        {
            _ = _target.SetPositionY(_zoomMin);
        }
    }

    private void ResetCamera()
    {
        _ = _target.SetPositionX(0);
        _ = _target.SetPositionZ(-120);
    }
}