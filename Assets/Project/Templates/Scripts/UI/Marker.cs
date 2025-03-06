using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class Marker : MonoBehaviour
{
    [SerializeField] private CameraOperator _camera; // Камера, относительно которой позиционируется иконка

    [SerializeField] private Transform _target; // Объект в мировом пространстве
    [SerializeField] [Min(0)] private float _offset = 50f; // Отступ от края экрана (в пикселях)

    private RectTransform _iconRect;
    [SerializeField] private Transform _arrow;

    void Start()
    {
        _iconRect = GetComponent<RectTransform>();
        _camera.OnUpdatePosition += UpdateMarkerPosition;
    }

    void OnDestroy()
    {
        _camera.OnUpdatePosition -= UpdateMarkerPosition;
    }

    public void UpdateTarget(Vector3 target)
    {
        _target.position = target;
    }

    private void UpdateMarkerPosition()
    {
        bool isOnScreen = _target.position.IsVisibleFrom(_camera.Camera, true, true, out Vector3 screenPos);

        if (_arrow) _arrow.gameObject.SetActive(!isOnScreen);
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
        float x = dir.x > 0 ? Screen.width - _offset : _offset;
        float y = screenCenter.y + slope * (x - screenCenter.x);

        if (y > Screen.height - _offset)
        {
            y = Screen.height - _offset;
            x = screenCenter.x + (y - screenCenter.y) / slope;
        }
        else if (y < _offset)
        {
            y = _offset;
            x = screenCenter.x + (y - screenCenter.y) / slope;
        }

        return new Vector3(x, y, 0);
    }

    private void RotateIconTowardsTarget(Transform tr, Vector3 screenPos)
    {
        if (!tr) return;
        // Направление от центра экрана к объекту
        Vector3 dir = (screenPos - new Vector3(Screen.width, Screen.height, 0) / 2).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        tr.rotation = Quaternion.Euler(0, 0, angle - 90); // Корректировка угла для стрелки
    }
}