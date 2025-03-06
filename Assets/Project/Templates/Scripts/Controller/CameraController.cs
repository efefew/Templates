using System;

using UnityEngine;

public abstract class CameraController
{
    public Action OnReset;
    public event Action<float> OnZoom, OnRotate;
    public event Action<Vector2> OnMove;
    protected ListKeyCombination _zoomIn, _zoomOut;
    protected ListKeyCombination _rotateClockwise, _rotateCounterclockwise;
    protected ListKeyCombination _moveUp, _moveDown, _moveLeft, _moveRight;
    protected ListKeyCombination _reset;
    public void Control()
    {
        float zoom = (_zoomIn != null ? _zoomIn.Get() : 0) - (_zoomOut != null ? _zoomOut.Get() : 0);
        float rotate = (_rotateClockwise != null ? _rotateClockwise.Get() : 0) - (_rotateCounterclockwise != null ? _rotateCounterclockwise.Get() : 0);
        float moveUpDown = (_moveUp != null ? _moveUp.Get() : 0) - (_moveDown != null ? _moveDown.Get() : 0);
        float moveRightLeft = (_moveRight != null ? _moveRight.Get() : 0) - (_moveLeft != null ? _moveLeft.Get() : 0);
        float reset = _reset != null ? _reset.Get() : 0;

        Zoom(zoom);
        Rotate(rotate);
        Move(moveRightLeft, moveUpDown);
        Reset(reset);
    }
    public void Zoom(float key)
    {
        if (key == 0)
        {
            return;
        }

        OnZoom?.Invoke(key);
    }

    public void Rotate(float key)
    {
        if (key == 0)
        {
            return;
        }

        OnRotate?.Invoke(key);
    }

    public void Move(float keyX, float keyY) => OnMove?.Invoke(new Vector2(keyX, keyY));

    public void Reset(float key)
    {
        if (key == 0)
        {
            return;
        }

        OnReset?.Invoke();
    }
}
