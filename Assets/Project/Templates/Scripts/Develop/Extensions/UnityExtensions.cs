using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

public static class UnityExtensions
{
    public static bool AnyContains(this string text, params string[] values)
    {
        for (int id = 0; id < values.Length; id++)
        {
            if (text.Contains(values[id]))
            {
                return true;
            }
        }

        return false;
    }
    public static Texture2D ToTexture2D(this byte[] bytes)
    {
        Texture2D texture = new(2, 2);
        _ = texture.LoadImage(bytes);
        texture.Apply();
        return texture;
    }
    public static Sprite ToSprite(this Texture2D tex) => Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
    /// <summary>
    /// Таймер
    /// </summary>
    /// <param name="timer">значение таймера</param>
    /// <returns>значение таймера равно нулю и не изменилось?</returns>
    public static bool Timer(this ref float timer)
    {
        if (timer == 0)
        {
            return true;
        }

        timer -= Time.fixedDeltaTime;
        if (timer < 0)
        {
            timer = 0;
        }

        return false;
    }
    /// <summary>
    /// Следить за целью только по оси Y
    /// </summary>
    /// <param name="transform">следящий</param>
    /// <param name="target">цель</param>
    public static void LookAtY(Transform transform, Vector3 target)
    {
        Vector3 targetPosition = new(target.x, transform.position.y, target.z);
        transform.LookAt(targetPosition);
    }
    /// <summary>
    /// Следить за целью (2D версия)
    /// </summary>
    /// <param name="transform">следящий</param>
    /// <param name="target">цель</param>
    public static void LookAt2D(this Transform transform, Vector3 target, bool localAngle = false)
    {
        Vector2 direction = target - transform.position;
        float angle = Vector2.SignedAngle(Vector2.right, direction);
        _ = transform.SetAngleZ(angle, localAngle);
    }
    /// <summary>
    /// Попробовать получить значение другого типа
    /// </summary>
    /// <typeparam name="T">другой тип</typeparam>
    /// <param name="obj">исходное значение</param>
    /// <param name="valueOtherType">значение другого типа</param>
    /// <returns>Получилось ли получить значение другого типа</returns>
    public static bool TryGetValueOtherType<T>(this object obj, out T valueOtherType)
    {
        if (obj.GetType() == typeof(T))
        {
            valueOtherType = (T)obj;
            return true;
        }

        valueOtherType = default;
        return false;
    }
    public static void Clear(this Transform transform)
    {
        if (transform.childCount == 0)
        {
            return;
        }

        for (int idChild = 0; idChild < transform.childCount; idChild++)
        {
            UnityEngine.Object.Destroy(transform.GetChild(idChild).gameObject);
        }
    }

    /// <summary>
    /// Проверяет, находится ли указатель мыши над объектом UI.
    /// </summary>
    /// <returns>находится ли указатель мыши над объектом UI</returns>
    public static bool IsPointerOverUI()
    {
        // Создаём экземпляр PointerEventData с текущим положением указателя мыши.
        PointerEventData eventDataCurrentPosition = new(EventSystem.current)
        {
            position = new Vector2(Input.mousePosition.x, Input.mousePosition.y)
        };

        List<RaycastResult> results = new();

        // Выполняем лучевой кастинг для всех объектов в текущей позиции указателя мыши.
        // Результаты сохраняются в списке results.
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        // Если количество результатов больше нуля, значит указатель мыши находится над объектом UI.
        return results.Count > 0;
    }
    public static void SetLocal(this Transform tr, Vector3? position = null, Quaternion? rotation = null, Vector3? scale = null)
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
}