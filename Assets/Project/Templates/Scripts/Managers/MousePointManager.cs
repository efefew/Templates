using System;
using UnityEngine;

public class MousePointManager : MonoBehaviour
{
    private static Vector3 _screenPosition;
    public static Vector3 ScreenPosition
    {
        get
        {
#if (UNITY_ANDROID || UNITY_IOS)
            if(TargetCamera&& Input.touchCount > 0)
                _screenPosition = Input.GetTouch(0).position;
#endif
            return _screenPosition;
        }
        private set => _screenPosition = value;
    }

    private static Vector3 _worldPosition;

    public static Vector3 WorldPosition
    {
        get
        {
#if (UNITY_ANDROID || UNITY_IOS)
            if(TargetCamera && Input.touchCount > 0)
                _worldPosition = TargetCamera.ScreenToWorldPoint(Input.GetTouch(0).position);
#endif
            return _worldPosition;
        }
        private set
        {
            _worldPosition = value;
        }
    }
    [SerializeField]
    private Camera _c;
#if (UNITY_ANDROID || UNITY_IOS)
    public static Camera TargetCamera { get; private set; }

    private void Awake()
    {
        TargetCamera = _c;
    }
#else
    private void OnGUI()
    {
        Event e = Event.current;

        Vector2 mousePos = new()
        {
            x = e.mousePosition.x,
            y = _c.pixelHeight - e.mousePosition.y
        };

        ScreenPosition = new Vector3(mousePos.x, mousePos.y, _c.nearClipPlane);

        WorldPosition = _c.ScreenToWorldPoint(ScreenPosition);
    }
#endif
}
