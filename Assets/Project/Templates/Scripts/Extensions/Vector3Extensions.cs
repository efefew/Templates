using UnityEngine;

public static class Vector3Extensions
{
    #region Public Methods

    public static void AddAngleX(this Transform tr, float x) => tr.eulerAngles = tr.eulerAngles.AddX(x);

    public static void AddAngleY(this Transform tr, float y) => tr.eulerAngles = tr.eulerAngles.AddY(y);

    public static void AddAngleZ(this Transform tr, float z) => tr.eulerAngles = tr.eulerAngles.AddZ(z);

    public static void AddPositionX(this Transform tr, float x) => tr.position = tr.position.AddX(x);

    public static void AddPositionY(this Transform tr, float y) => tr.position = tr.position.AddY(y);

    public static void AddPositionZ(this Transform tr, float z) => tr.position = tr.position.AddZ(z);

    public static Vector3 AddX(this Vector3 vector, float x) => new(vector.x + x, vector.y, vector.z);

    public static Vector3 AddY(this Vector3 vector, float y) => new(vector.x, vector.y + y, vector.z);

    public static Vector3 AddZ(this Vector3 vector, float z) => new(vector.x, vector.y, vector.z + z);

    public static void SetAngleX(this Transform tr, float x) => tr.eulerAngles = tr.eulerAngles.SetX(x);

    public static void SetAngleY(this Transform tr, float y) => tr.eulerAngles = tr.eulerAngles.SetY(y);

    public static void SetAngleZ(this Transform tr, float z) => tr.eulerAngles = tr.eulerAngles.SetZ(z);

    public static void SetPositionX(this Transform tr, float x) => tr.position = tr.position.SetX(x);

    public static void SetPositionY(this Transform tr, float y) => tr.position = tr.position.SetY(y);

    public static void SetPositionZ(this Transform tr, float z) => tr.position = tr.position.SetZ(z);

    public static Vector2 SetX(this Vector2 vector, float x) => new(x, vector.y);

    public static Vector3 SetX(this Vector3 vector, float x) => new(x, vector.y, vector.z);

    public static Vector2 SetY(this Vector2 vector, float y) => new(vector.x, y);

    public static Vector3 SetY(this Vector3 vector, float y) => new(vector.x, y, vector.z);

    public static Vector3 SetZ(this Vector3 vector, float z) => new(vector.x, vector.y, z);

    #endregion Public Methods
}