using System;
using UnityEngine;
public class Swipe : MonoBehaviour
{
    public static event Action<Vector2> OnSwipe;
    private bool _isSwiping;
    private Vector3 _pointStart;
    
    private void Update()
    {
        CheckStartSwipe();
        CheckEndSwipe();
    }

    private void CheckEndSwipe()
    {
        if(!_isSwiping) return;
        if (!Input.GetKey(KeyCode.Mouse0)) CheckMove();
    }
    private void CheckMove()
    {
        _isSwiping = false;
        Vector2 direction = MousePointManager.WorldPosition - _pointStart;
        float force = direction.magnitude;
        direction.Normalize();
        if(force < MIN_FORCE) return;
        Move(direction, force);
    }

    private void CheckStartSwipe()
    {
        if(_isSwiping) return;
        if (Input.touchCount != 1) return;
        _isSwiping = true;
        _pointStart = MousePointManager.WorldPosition;
    }

    private static void Move(Vector2 direction, float force)
    {
        force = Mathf.Min(force, MAX_FORCE);
        OnSwipe?.Invoke(direction * force);
    }

    private const float MAX_FORCE = 10;
    public const float MIN_FORCE = 1;
}