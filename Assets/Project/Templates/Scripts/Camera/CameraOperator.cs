using UnityEngine;

public abstract class CameraOperator : MonoBehaviour, BootstrapElement
{
    [SerializeField]
    protected float _speedMove = 3, _speedRotate = 1, _speedZoom = 1, _zoomMin = 100, _zoomMax = 200;
    [SerializeField]
    [Min(0)]
    protected float _lerpMove = 0.01f, _lerpRotate = 0.01f;
    [SerializeField]
    protected Vector3 _defaultPosition;
    protected Controller _controller;
    protected Transform _trCamera;
    public virtual void StartBootstrap()
    {
        _trCamera = transform;
        _controller = Controller.Instance;
        _controller.CameraController.OnMove += Move;
        _controller.CameraController.OnRotate += Rotate;
        _controller.CameraController.OnZoom += Zoom;
        _controller.CameraController.OnReset += ResetCamera;
        ResetCamera();
    }
    private void Update() => _controller.CameraController.Control();
    private void LateUpdate() => CameraUpdate();
    protected abstract void CameraUpdate();
    protected abstract void Move(Vector2 position);
    protected abstract void Rotate(float scaleRotation);
    protected abstract void Zoom(float scaleZoom);
    protected abstract void ResetCamera();
}

