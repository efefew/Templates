using UnityEngine;

public static class TransformExtensions
{
    public static void SetLocal(this Transform tr, Vector3? position = null, Quaternion? rotation = null,
        Vector3? scale = null)
    {
        if (position != null)
        {
            tr.localPosition = (Vector3)position;
        }

        if (rotation != null)
        {
            tr.localRotation = (Quaternion)rotation;
        }

        if (scale != null)
        {
            tr.localScale = (Vector3)scale;
        }
    }
    public static void Clear(this Transform transform)
    {
        if (transform.childCount == 0)
        {
            return;
        }

        for (int idChild = 0; idChild < transform.childCount; idChild++)
        {
            if (Application.isEditor)
                Object.DestroyImmediate(transform.GetChild(idChild).gameObject);
            else
                Object.Destroy(transform.GetChild(idChild).gameObject);
        }
    }
    
    /// <summary>
    /// Получить угол слежения за целью только по оси Y
    /// </summary>
    /// <param name="transform">следящий</param>
    /// <param name="target">цель</param>
    public static Quaternion GetQuaternionLookAtY(this Transform transform, Vector3 target)
    {
        Vector3 targetPosition = new(target.x, transform.position.y, target.z);
        Quaternion oldQuaternion = transform.rotation;
        transform.LookAt(targetPosition);
        Quaternion quaternion = transform.rotation;
        transform.rotation = oldQuaternion;
        return quaternion;
    }

    /// <summary>
    /// Следить за целью только по оси Y
    /// </summary>
    /// <param name="transform">следящий</param>
    /// <param name="target">цель</param>
    public static void LookAtY(this Transform transform, Vector3 target)
    {
        Vector3 targetPosition = new(target.x, transform.position.y, target.z);
        transform.LookAt(targetPosition);
    }

    /// <summary>
    /// Следить за целью (2D версия)
    /// </summary>
    /// <param name="transform">следящий</param>
    /// <param name="target">цель</param>
    /// <param name="localAngle">Это локальный угол?</param>
    public static void LookAt2D(this Transform transform, Vector3 target, bool localAngle = false)
    {
        Vector2 direction = target - transform.position;
        float angle = Vector2.SignedAngle(Vector2.right, direction);
        _ = transform.SetAngleZ(angle, localAngle);
    }
    
    public static void SetInsideBorders(this Transform tr, (Vector2 min, Vector2 max) point)
    {
        Vector3 position = tr.position;

        if (point.min.x > position.x) tr.SetPositionX(point.min.x);
        if (point.min.y > position.y) tr.SetPositionY(point.min.y);
        if (point.max.x < position.x) tr.SetPositionX(point.max.x);
        if (point.max.y < position.y) tr.SetPositionY(point.max.y);
    }
    public static void SetInsideBorders3D(this Transform tr, (Vector2 min, Vector2 max) point)
    {
        Vector3 position = tr.position;

        if (point.min.x > position.x) tr.SetPositionX(point.min.x);
        if (point.min.y > position.z) tr.SetPositionZ(point.min.y);
        if (point.max.x < position.x) tr.SetPositionX(point.max.x);
        if (point.max.y < position.z) tr.SetPositionZ(point.max.y);
    }
}
