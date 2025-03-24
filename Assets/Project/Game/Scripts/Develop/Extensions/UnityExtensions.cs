using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public static class UnityExtensions
{
    public static bool AnyContains(this string text, params string[] values)
    {
        return values.Any(text.Contains);
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
    /// <param name="timer">Значение таймера</param>
    /// <returns>Значение таймера равно нулю и не изменилось?</returns>
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
    /// <param name="localAngle">Это локальный угол?</param>
    public static void LookAt2D(this Transform transform, Vector3 target, bool localAngle = false)
    {
        Vector2 direction = target - transform.position;
        float angle = Vector2.SignedAngle(Vector2.right, direction);
        _ = transform.SetAngleZ(angle, localAngle);
    }
    /// <summary>
    /// Попробовать получить значение другого типа
    /// </summary>
    /// <typeparam name="T">Другой тип</typeparam>
    /// <param name="obj">Исходное значение</param>
    /// <param name="valueOtherType">Значение другого типа</param>
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
            Object.Destroy(transform.GetChild(idChild).gameObject);
        }
    }

    /// <summary>
    /// Проверяет, находится ли указатель мыши над объектом UI.
    /// </summary>
    /// <returns>Находится ли указатель мыши над объектом UI</returns>
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
    /// <summary>
    /// Находится ли объект в пределах экрана и не перекрыт
    /// </summary>
    /// <param name="transform">объект</param>
    /// <param name="camera">камера</param>
    /// <returns></returns>
    public static bool IsRayVisibleFrom(this Transform transform, Camera camera)
    {
        Ray ray = camera.ScreenPointToRay(camera.WorldToScreenPoint(transform.position));
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.transform == transform;
        }
        
        return false;
    }

    /// <summary>
    ///  Находится ли объект в пределах экрана
    /// </summary>
    /// <param name="position">объект</param>
    /// <param name="camera">камера</param>
    /// <param name="invert">Если объект позади камеры, инвертируем координаты</param>
    /// <param name="world">Мировые координаты</param>
    /// <param name="screenPos">Позиция объекта на экране</param>
    /// <returns></returns>
    public static bool IsVisibleFrom(this Vector3 position, Camera camera, bool invert, bool world, out Vector3 screenPos)
    {
        screenPos = camera.WorldToViewportPoint(position);
        int width = 1, height = 1;
        if (world)
        {
            width = Screen.width;
            height = Screen.height;
            screenPos = screenPos.MulX(width).MulY(height);
        }
        if (invert && screenPos.z < 0) screenPos *= -1;
        
        return 
            screenPos.x > 0 && screenPos.x < width &&
            screenPos.y > 0 && screenPos.y < height &&
            screenPos.z > 0; // Z > 0 означает, что объект перед камерой
    }
    /// <summary>
    ///  Находится ли объект в пределах экрана
    /// </summary>
    /// <param name="position">объект</param>
    /// <param name="camera">камера</param>
    /// <param name="invert">Если объект позади камеры, инвертируем координаты</param>
    /// <param name="world">Мировые координаты</param>
    /// <returns></returns>
    public static bool IsVisibleFrom(this Vector3 position, Camera camera, bool invert = false, bool world = false)
    {
        Vector3 screenPos = camera.WorldToViewportPoint(position);
        if (world) screenPos.MulX(Screen.width).MulY(Screen.height);
        if (invert && screenPos.z < 0) screenPos *= -1;
        
        return 
            screenPos.x > 0 && screenPos.x < Screen.width &&
            screenPos.y > 0 && screenPos.y < Screen.height &&
            screenPos.z > 0; // Z > 0 означает, что объект перед камерой
    }
    /// <summary>
    /// Проверка пересечения с Frustum камеры 
    /// </summary>
    /// <param name="renderer">объект</param>
    /// <param name="camera">frustum камера</param>
    /// <returns></returns>
    public static bool FrustumCheck(Renderer renderer, Camera camera)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
        return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
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