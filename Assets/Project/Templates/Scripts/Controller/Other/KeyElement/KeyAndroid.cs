using UnityEngine;

public class KeyAndroid : Key
{
    public enum TypeSmooth
    {
        None,
        ZoomIn,
        ZoomOut,
        RotateClockwise,
        RotateCounterclockwise,
        MoveUp,
        MoveDown,
        MoveRight,
        MoveLeft,
    }
    private TypeSmooth _smoothKey;

    public KeyAndroid(TypeSmooth smoothKey, float scale = 1f)
    {
        _scale = scale;
        _smoothKey = smoothKey;
    }

    public override float Get() => GetSmoothClick();
    private float GetSmoothClick()
    {
        return _smoothKey switch
        {
            TypeSmooth.None => 0,
            TypeSmooth.ZoomIn => Mathf.Max(0, Zoom()),
            TypeSmooth.ZoomOut => -Mathf.Min(0, Zoom()),
            TypeSmooth.RotateClockwise => Mathf.Max(0, Rotate()),
            TypeSmooth.RotateCounterclockwise => -Mathf.Min(0, Rotate()),
            TypeSmooth.MoveUp => Mathf.Max(0, MoveUpDown()),
            TypeSmooth.MoveDown => -Mathf.Min(0, MoveUpDown()),
            TypeSmooth.MoveRight => Mathf.Max(0, MoveRightLeft()),
            TypeSmooth.MoveLeft => -Mathf.Min(0, MoveRightLeft()),
            _ => 0,
        };
    }
    private float Zoom()
    {
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroLastPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOneLastPos = touchOne.position - touchOne.deltaPosition;
            float distance = (touchZeroLastPos - touchOneLastPos).magnitude;
            float currentDistance = (touchZero.position - touchOne.position).magnitude;
            float difference = currentDistance - distance;
            return difference;
        }

        return 0;
    }
    private float Rotate()
    {
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroLastPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOneLastPos = touchOne.position - touchOne.deltaPosition;
            float angle = Vector2.Angle(touchZeroLastPos, touchOneLastPos);
            float currentAngle = Vector2.Angle(touchZero.position, touchOne.position);
            float difference = currentAngle - angle;
            return difference;
        }

        return 0;
    }
    private float MoveUpDown() => Input.touchCount > 0 ? Input.GetTouch(0).position.y : 0;
    private float MoveRightLeft() => Input.touchCount > 0 ? Input.GetTouch(0).position.x : 0;
}
