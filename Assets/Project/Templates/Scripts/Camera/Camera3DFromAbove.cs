using UnityEngine;

public class Camera3DFromAbove : CameraOperator
{

    private const float MIN_DELTA = 0.1f;
    private const int CIRCLE_ANGLE = 360;
    [SerializeField]
    private Transform _target;

    [SerializeField]
    private float _distanceZ = 5, _angleX = 30, _zoomDistance = 1f;
    [SerializeField]
    private float _defaultAngleY, _defaultZoom;
    private float _targetAngleY;

    protected override void CameraUpdate()
    {
        Vector3 angle = Vector3.zero.SetY(_targetAngleY);
        _target.rotation = Quaternion.Slerp(_target.rotation, Quaternion.Euler(angle), _lerpRotate);
        _trCamera.position -= ShiftCamera();
        _trCamera.position = Vector3.Lerp(_trCamera.position, _target.position, _lerpMove);
        _trCamera.eulerAngles = _target.eulerAngles.SetX(_angleX);
        _trCamera.position += ShiftCamera();
        return;

        Vector3 ShiftCamera() => _trCamera.Forward(_trCamera.eulerAngles.SetX(0), -_distanceZ) + _trCamera.Forward(_trCamera.eulerAngles.SetX(_angleX), -_zoomDistance);
    }

    protected override void Move(Vector2 position)
    {
        if (position == Vector2.zero)
        {
            return;
        }
        float oldY = _target.position.y;
        _target.position -= _target.right * (position.x * _speedMove);
        _target.position -= _target.forward * (position.y * _speedMove);
        _ = _target.SetPositionY(oldY);
    }
    protected override void Rotate(float scaleRotation)
    {
        if (scaleRotation == 0)
        {
            return;
        }

        _targetAngleY += scaleRotation * _speedRotate;
        if (_targetAngleY is < (-CIRCLE_ANGLE) or > CIRCLE_ANGLE)
        {
            _targetAngleY = 0;
        }
    }
    protected override void Zoom(float scaleZoom)
    {
        if (scaleZoom == 0)
        {
            return;
        }

        _zoomDistance += scaleZoom * _speedZoom;
        if (_zoomDistance > _zoomMax + MIN_DELTA)
        {
            _zoomDistance = _zoomMax;
        }

        if (_zoomDistance < _zoomMin - MIN_DELTA)
        {
            _zoomDistance = _zoomMin;
        }
    }

    protected override void ResetCamera()
    {
        _target.position = _defaultPosition;
        _targetAngleY = _defaultAngleY;
        _zoomDistance = _defaultZoom;
        CameraUpdate();
    }
}
