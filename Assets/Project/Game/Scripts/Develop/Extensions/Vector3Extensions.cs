using System.Linq;
using UnityEngine;

public static class MultiplicationVector3Extensions
{
    public static Vector3 MulX(this Vector2 vector, float value) => new(vector.x * value, vector.y);
    public static Vector3 MulY(this Vector2 vector, float value) => new(vector.x, vector.y  * value);
    public static Vector3 MulX(this Vector3 vector, float value) => new(vector.x * value, vector.y, vector.z);
    public static Vector3 MulY(this Vector3 vector, float value) => new(vector.x, vector.y  * value, vector.z);
    public static Vector3 MulZ(this Vector3 vector, float value) => new(vector.x, vector.y, vector.z  * value);
}
public static class AxisVector3Extensions
{
    public static Vector3 Forward(this Transform tr, Vector3 angle, float scale)
    {
        Quaternion quaternion = tr.rotation;
        tr.eulerAngles = angle;
        Vector3 vector = tr.forward * scale;
        tr.rotation = quaternion;
        return vector;
    }
    public static Vector3 Up(this Transform tr, Vector3 angle, float scale)
    {
        Quaternion quaternion = tr.rotation;
        tr.eulerAngles = angle;
        Vector3 vector = tr.up * scale;
        tr.rotation = quaternion;
        return vector;
    }
    public static Vector3 Right(this Transform tr, Vector3 angle, float scale)
    {
        Quaternion quaternion = tr.rotation;
        tr.eulerAngles = angle;
        Vector3 vector = tr.right * scale;
        tr.rotation = quaternion;
        return vector;
    }
    public static Vector3 Forward(this Transform tr, Quaternion angle, float scale)
    {
        Quaternion quaternion = tr.rotation;
        tr.rotation = angle;
        Vector3 vector = tr.forward * scale;
        tr.rotation = quaternion;
        return vector;
    }
    public static Vector3 Up(this Transform tr, Quaternion angle, float scale)
    {
        Quaternion quaternion = tr.rotation;
        tr.rotation = angle;
        Vector3 vector = tr.up * scale;
        tr.rotation = quaternion;
        return vector;
    }
    public static Vector3 Right(this Transform tr, Quaternion angle, float scale)
    {
        Quaternion quaternion = tr.rotation;
        tr.rotation = angle;
        Vector3 vector = tr.right * scale;
        tr.rotation = quaternion;
        return vector;
    }
}
public static class AddVector3Extensions
{
    public static Transform AddAngleX(this Transform tr, float x, bool local = false)
    {
        if (local)
        {
            tr.localEulerAngles = tr.localEulerAngles.AddX(x);
        }
        else
        {
            tr.eulerAngles = tr.eulerAngles.AddX(x);
        }

        return tr;
    }

    public static Transform AddAngleY(this Transform tr, float y, bool local = false)
    {
        if (local)
        {
            tr.localEulerAngles = tr.localEulerAngles.AddY(y);
        }
        else
        {
            tr.eulerAngles = tr.eulerAngles.AddY(y);
        }

        return tr;
    }

    public static Transform AddAngleZ(this Transform tr, float z, bool local = false)
    {
        if (local)
        {
            tr.localEulerAngles = tr.localEulerAngles.AddZ(z);
        }
        else
        {
            tr.eulerAngles = tr.eulerAngles.AddZ(z);
        }

        return tr;
    }

    public static Transform AddPositionX(this Transform tr, float x, bool local = false)
    {
        if (local)
        {
            tr.localPosition = tr.localPosition.AddX(x);
        }
        else
        {
            tr.position = tr.position.AddX(x);
        }

        return tr;
    }

    public static Transform AddPositionY(this Transform tr, float y, bool local = false)
    {
        if (local)
        {
            tr.localPosition = tr.localPosition.AddY(y);
        }
        else
        {
            tr.position = tr.position.AddY(y);
        }

        return tr;
    }

    public static Transform AddPositionZ(this Transform tr, float z, bool local = false)
    {
        if (local)
        {
            tr.localPosition = tr.localPosition.AddZ(z);
        }
        else
        {
            tr.position = tr.position.AddZ(z);
        }

        return tr;
    }
    public static Vector2 AddX(this Vector2 vector, float x) => new(vector.x + x, vector.y);

    public static Vector2 AddY(this Vector2 vector, float y) => new(vector.x, vector.y + y);
    public static Vector3 AddX(this Vector3 vector, float x) => new(vector.x + x, vector.y, vector.z);

    public static Vector3 AddY(this Vector3 vector, float y) => new(vector.x, vector.y + y, vector.z);

    public static Vector3 AddZ(this Vector3 vector, float z) => new(vector.x, vector.y, vector.z + z);
}
public static class SetVector3Extensions
{
    public static Transform SetAngleX(this Transform tr, float x, bool local = false)
    {
        if (local)
        {
            tr.localEulerAngles = tr.localEulerAngles.SetX(x);
        }
        else
        {
            tr.eulerAngles = tr.eulerAngles.SetX(x);
        }

        return tr;
    }

    public static Transform SetAngleY(this Transform tr, float y, bool local = false)
    {
        if (local)
        {
            tr.localEulerAngles = tr.localEulerAngles.SetY(y);
        }
        else
        {
            tr.eulerAngles = tr.eulerAngles.SetY(y);
        }

        return tr;
    }

    public static Transform SetAngleZ(this Transform tr, float z, bool local = false)
    {
        if (local)
        {
            tr.localEulerAngles = tr.localEulerAngles.SetZ(z);
        }
        else
        {
            tr.eulerAngles = tr.eulerAngles.SetZ(z);
        }

        return tr;
    }

    public static Transform SetPositionX(this Transform tr, float x, bool local = false)
    {
        if (local)
        {
            tr.localPosition = tr.localPosition.SetX(x);
        }
        else
        {
            tr.position = tr.position.SetX(x);
        }

        return tr;
    }

    public static Transform SetPositionY(this Transform tr, float y, bool local = false)
    {
        if (local)
        {
            tr.localPosition = tr.localPosition.SetY(y);
        }
        else
        {
            tr.position = tr.position.SetY(y);
        }

        return tr;
    }

    public static Transform SetPositionZ(this Transform tr, float z, bool local = false)
    {
        if (local)
        {
            tr.localPosition = tr.localPosition.SetZ(z);
        }
        else
        {
            tr.position = tr.position.SetZ(z);
        }

        return tr;
    }

    public static Vector2 SetX(this Vector2 vector, float x) => new(x, vector.y);

    public static Vector3 SetX(this Vector3 vector, float x) => new(x, vector.y, vector.z);

    public static Vector2 SetY(this Vector2 vector, float y) => new(vector.x, y);

    public static Vector3 SetY(this Vector3 vector, float y) => new(vector.x, y, vector.z);

    public static Vector3 SetZ(this Vector3 vector, float z) => new(vector.x, vector.y, z);
}
public static class ConvertVector3Extensions
{
    public static Vector2 ZToY(this Vector3 vector) => new(vector.x, vector.z);
    public static Vector2Int ToVector2Int(this Vector2 vector) => new ((int)vector.x, (int)vector.y);
}
public static class Vector3Extensions
{
    public static Vector3 GetCenter(params Transform[] points)
    {
        Vector3 center = points.Aggregate(Vector3.zero, (current, t) => current + t.position);

        center /= points.Length;
        return center;
    }
    public static Vector3 GetCenter(params Vector3[] points)
    {
        Vector3 center = points.Aggregate(Vector3.zero, (current, t) => current + t);

        center /= points.Length;
        return center;
    }
    public static Vector2 GetCenter(params Vector2[] points)
    {
        Vector2 center = points.Aggregate(Vector2.zero, (current, t) => current + t);

        center /= points.Length;
        return center;
    }
}