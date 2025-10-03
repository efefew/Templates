#region

using UnityEngine;
using UnityEngine.UI;

#endregion

public class Marker : MonoBehaviour
{
    /// <summary>
    ///     Камера, относительно которой позиционируется иконка
    /// </summary>
    [SerializeField] private CameraOperator _camera;

    /// <summary>
    ///     Объект в мировом пространстве
    /// </summary>
    [SerializeField] private Transform _target;

    /// <summary>
    ///     Отступ от края экрана (в пикселях)
    /// </summary>
    [SerializeField, Min(0)] private float _offset;
    [SerializeField, Min(0)] private float _addOffsetLeftUp;

    /// <summary>
    ///     Отступ от точки
    /// </summary>
    [SerializeField] private Vector3 _offsetPosition;

    [SerializeField] private GameObject _markerObject;
    [SerializeField] private Image _centerImage;
    [SerializeField] private Transform _arrow;
    [SerializeField] private bool _showCenterOutside;
    private RectTransform _iconRect;

    private void Start()
    {
        _iconRect = GetComponent<RectTransform>();
        _markerObject.gameObject.SetActive(_target);
        _camera.OnUpdatePosition += UpdateMarkerPosition;
    }

    private void OnDestroy()
    {
        _camera.OnUpdatePosition -= UpdateMarkerPosition;
    }

    public void ClearTarget()
    {
        _target = null;
        _markerObject.gameObject.SetActive(false);
    }

    public void SetTarget(Transform target, Vector3 offsetPosition)
    {
        _offsetPosition = offsetPosition;
        _target = target;
        _markerObject.gameObject.SetActive(_target);
        UpdateMarkerPosition();
    }

    private void UpdateMarkerPosition()
    {
        if (!_target) return;

        bool isOnScreen =
            (_target.position + _offsetPosition).IsVisibleFrom(_camera.Camera, true, true, out Vector3 screenPos);

        if (_arrow) _arrow.gameObject.SetActive(!isOnScreen);
        if (!_showCenterOutside) _centerImage.enabled = isOnScreen;

        if (isOnScreen)
        {
            // Если объект на экране, размещаем иконку поверх него
            _iconRect.position = screenPos;
        }
        else
        {
            // Если объект вне экрана, вычисляем позицию на краю
            Vector3 edgePosition = GetEdgePosition(screenPos);
            _iconRect.position = edgePosition;

            // Поворачиваем иконку в направлении объекта (опционально)
            RotateIconTowardsTarget(_arrow, screenPos);
        }
    }

    private Vector3 GetEdgePosition(Vector3 screenPos)
    {
        // Центр экрана
        Vector3 screenCenter = new Vector3(Screen.width, Screen.height, 0) / 2;

        // Направление от центра к позиции объекта
        Vector3 dir = (screenPos - screenCenter).normalized;

        // Находим пересечение с границами экрана
        float angle = Mathf.Atan2(dir.y, dir.x);
        float slope = Mathf.Tan(angle);

        // Вычисляем позицию на краю экрана с учетом отступа
        float x = dir.x > 0 ? Screen.width - _offset : _offset + _addOffsetLeftUp;
        float y = screenCenter.y + slope * (x - screenCenter.x);

        if (y > Screen.height - _offset - _addOffsetLeftUp)
        {
            y = Screen.height - _offset - _addOffsetLeftUp;
            x = screenCenter.x + (y - screenCenter.y) / slope;
        }
        else if (y < _offset)
        {
            y = _offset;
            x = screenCenter.x + (y - screenCenter.y) / slope;
        }

        return new Vector3(x, y, 0);
    }

    private static void RotateIconTowardsTarget(Transform tr, Vector3 screenPos)
    {
        if (!tr) return;
        // Направление от центра экрана к объекту
        Vector3 dir = (screenPos - new Vector3(Screen.width, Screen.height, 0) / 2).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        tr.rotation = Quaternion.Euler(0, 0, angle - 90); // Корректировка угла для стрелки
    }
}