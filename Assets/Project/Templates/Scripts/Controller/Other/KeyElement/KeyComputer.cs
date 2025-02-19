using UnityEngine;

public class KeyComputer : Key
{
    public enum TypeSmooth
    {
        None,
        MouseUp,
        MouseDown,
        MouseRight,
        MouseLeft,
        ScrollUp,
        ScrollDown
    }

    private KeyCode _key;
    private TypeSmooth _smoothKey;
    private TypeClick _typeClick;
    public KeyComputer(TypeSmooth smoothKey, float scale = 1f)
    {
        _scale = scale;
        _key = KeyCode.None;
        _smoothKey = smoothKey;
    }

    public KeyComputer(TypeClick typeClick, KeyCode key)
    {
        _typeClick = typeClick;
        _key = key;
        _smoothKey = TypeSmooth.None;
    }

    public override float Get() => _key != KeyCode.None ? GetClick().ToInt() : GetSmoothClick() * _scale;

    private float GetSmoothClick()
    {
        return _smoothKey switch
        {
            TypeSmooth.None => 0,
            TypeSmooth.MouseUp => Mathf.Max(0, Input.GetAxis("Mouse Y")),
            TypeSmooth.MouseDown => -Mathf.Min(0, Input.GetAxis("Mouse Y")),
            TypeSmooth.MouseRight => Mathf.Max(0, Input.GetAxis("Mouse X")),
            TypeSmooth.MouseLeft => -Mathf.Min(0, Input.GetAxis("Mouse X")),
            TypeSmooth.ScrollUp => Mathf.Max(0, Input.GetAxis("Mouse ScrollWheel")),
            TypeSmooth.ScrollDown => -Mathf.Min(0, Input.GetAxis("Mouse ScrollWheel")),
            _ => 0,
        };
    }
    private bool GetClick()
    {
        return _typeClick switch
        {
            TypeClick.Down => Input.GetKeyDown(_key),
            TypeClick.Hold => Input.GetKey(_key),
            TypeClick.Up => Input.GetKeyUp(_key),
            _ => false,
        };
    }
}