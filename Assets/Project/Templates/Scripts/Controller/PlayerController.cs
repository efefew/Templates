using System;

using UnityEngine;

public abstract class PlayerController
{
    public Action<Vector2> OnMove;
    public abstract void Move();
}
