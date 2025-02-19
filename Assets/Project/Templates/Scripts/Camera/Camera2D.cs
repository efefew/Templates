using UnityEngine;

[RequireComponent(typeof(Camera))]
public class Camera2D : MonoBehaviour
{

    private Transform _tr;
    private Camera _camera;
    [Range(0f, 100f)]
    [SerializeField]
    private float moveButton, moveMouse, zoomButton, zoomScroll;
    [SerializeField]
    private float _moveButtonScale = 1000,
                  _moveMouseScale = 500,
                  _zoomButtonScale = 1000,
                  _zoomScrollScale = 1000;
    [SerializeField]
    private float _minOrthographicSize = 3;

    #region Unity Methods

    private void Start()
    {
        _camera = GetComponent<Camera>();
        _tr = transform;
        _tr.position = new Vector3(13, 11, -10);
        _camera.orthographicSize = 14;
    }
    private void Update()
    {

        if (Input.GetAxis("Mouse ScrollWheel") >= 0.1 && _camera.orthographicSize > _minOrthographicSize)
        {
            _camera.orthographicSize -= zoomScroll * _camera.orthographicSize / _zoomScrollScale;
        }

        if (Input.GetAxis("Mouse ScrollWheel") <= -0.1)
        {
            _camera.orthographicSize += zoomScroll * _camera.orthographicSize / _zoomScrollScale;
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            _tr.position -= _tr.up * Input.GetAxis("Mouse Y") * moveMouse * _camera.orthographicSize / _moveMouseScale;
            _tr.position -= _tr.right * Input.GetAxis("Mouse X") * moveMouse * _camera.orthographicSize / _moveMouseScale;
        }
    }
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Equals) && _camera.orthographicSize > _minOrthographicSize)
        {
            _camera.orthographicSize -= zoomButton * _camera.orthographicSize / _zoomButtonScale;
        }

        if (Input.GetKey(KeyCode.Minus))
        {
            _camera.orthographicSize += zoomButton * _camera.orthographicSize / _zoomButtonScale;
        }

        if (Input.anyKey)
        {
            float force = moveButton * _camera.orthographicSize / _moveButtonScale;
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

    #endregion Unity Methods
}