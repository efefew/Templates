using UnityEngine;
using UnityEngine.Serialization;

namespace Project.Templates.Scripts.Camera
{
    [RequireComponent(typeof(UnityEngine.Camera))]
    public class Camera2D : CameraOperator
    {

        private Transform _tr;
        private UnityEngine.Camera _camera;
        [FormerlySerializedAs("moveButton")]
        [Range(0f, 100f)]
        [SerializeField]
        private float _moveButton;

        [FormerlySerializedAs("moveMouse")]
        [Range(0f, 100f)]
        [SerializeField]
        private float _moveMouse;

        [FormerlySerializedAs("zoomButton")]
        [Range(0f, 100f)]
        [SerializeField]
        private float _zoomButton;

        [FormerlySerializedAs("zoomScroll")]
        [Range(0f, 100f)]
        [SerializeField]
        private float _zoomScroll;

        [SerializeField]
        private float _moveButtonScale = 1000,
            _moveMouseScale = 500,
            _zoomButtonScale = 1000,
            _zoomScrollScale = 1000;
        [SerializeField]
        private float _minOrthographicSize = 3;

        #region Unity Methods

        public override void StartBootstrap()
        {
            base.StartBootstrap();
            _camera = GetComponent<UnityEngine.Camera>();
            _tr = transform;
            _tr.position = new Vector3(13, 11, -10);
            _camera.orthographicSize = 14;
        }
        private void Update()
        {

            if (Input.GetAxis("Mouse ScrollWheel") >= 0.1 && _camera.orthographicSize > _minOrthographicSize)
            {
                _camera.orthographicSize -= _zoomScroll * _camera.orthographicSize / _zoomScrollScale;
            }

            if (Input.GetAxis("Mouse ScrollWheel") <= -0.1)
            {
                _camera.orthographicSize += _zoomScroll * _camera.orthographicSize / _zoomScrollScale;
            }

            if (Input.GetKey(KeyCode.Mouse1))
            {
                _tr.position -= _tr.up * Input.GetAxis("Mouse Y") * _moveMouse * _camera.orthographicSize / _moveMouseScale;
                _tr.position -= _tr.right * Input.GetAxis("Mouse X") * _moveMouse * _camera.orthographicSize / _moveMouseScale;
            }
        }
        private void FixedUpdate()
        {
            if (Input.GetKey(KeyCode.Equals) && _camera.orthographicSize > _minOrthographicSize)
            {
                _camera.orthographicSize -= _zoomButton * _camera.orthographicSize / _zoomButtonScale;
            }

            if (Input.GetKey(KeyCode.Minus))
            {
                _camera.orthographicSize += _zoomButton * _camera.orthographicSize / _zoomButtonScale;
            }

            if (Input.anyKey)
            {
                float force = _moveButton * _camera.orthographicSize / _moveButtonScale;
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                {
                    _tr.position -= transform.right * force;
                }

                if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                {
                    _tr.position += transform.right * force;
                }

                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                {
                    _tr.position += transform.up * force;
                }

                if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                {
                    _tr.position -= transform.up * force;
                }

                if (Input.GetKey(KeyCode.F) || Input.GetKey(KeyCode.Space))
                {
                    _tr.position = new Vector3(13, 11, -10);
                    _camera.orthographicSize = 14;
                }
            }
        }

        protected override void CameraUpdate() => throw new System.NotImplementedException();
        protected override void Move(Vector2 position) => throw new System.NotImplementedException();
        protected override void Rotate(float scaleRotation) => throw new System.NotImplementedException();
        protected override void Zoom(float scaleZoom) => throw new System.NotImplementedException();
        protected override void ResetCamera() => throw new System.NotImplementedException();

        #endregion Unity Methods
    }
}