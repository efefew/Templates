#region

using System;
using System.Collections;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem.XInput;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static CoroutineExtensions;
using static UnityExtensions;
using Object = UnityEngine.Object;

#endregion

public static class CoroutineExtensions
{
    private static int _countTemporaryVibration;

    public static IEnumerator ILoadScene(SceneType scene, Action<float> callback = null)
    {
        yield return SaveManager.LoadAll();
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene.ToString());
        while (operation is { isDone: false })
        {
            callback?.Invoke(operation.progress);
            yield return null;
        }
    }
    public static IEnumerator ITemporaryVibration(this XInputController xbox, float lowFrequency, float highFrequency,
        float duration)
    {
        _countTemporaryVibration++;
        xbox.SetMotorSpeeds(lowFrequency, highFrequency);
        yield return new WaitForSeconds(duration);
        _countTemporaryVibration--;
        if (_countTemporaryVibration == 0)
            xbox.SetMotorSpeeds(0, 0);
    }
}

public static class UnityExtensions
{
    public enum SceneType
    {
        Intro,

        /*Menu,
        Load,
        Lobby,*/
        Game
    }

    public static void TemporaryVibration(this MonoBehaviour monoBehaviour, XInputController xbox, float lowFrequency,
        float highFrequency, float duration)
    {
        monoBehaviour.StartCoroutine(xbox.ITemporaryVibration(lowFrequency, highFrequency, duration));
    }

    public static Vector3? ScreenTo3DPoint(this Camera camera)
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        Plane plane = new(Vector3.up, Vector3.zero);
        if (!plane.Raycast(ray, out float enter)) return null;
        return ray.GetPoint(enter);
    }

    public static Vector3? ScreenTo3DPoint(this Camera camera, Vector3 position)
    {
        Ray ray = camera.ScreenPointToRay(position);
        Plane plane = new(Vector3.up, Vector3.zero);
        if (!plane.Raycast(ray, out float enter)) return null;
        return ray.GetPoint(enter);
    }

    public static T TryGetEnum<T>(this string value) where T : struct, Enum
    {
        if (Enum.TryParse(value, out T result))
            return result;

        throw new ArgumentOutOfRangeException(nameof(value), value, null);
    }

    public static SceneType GetActiveScene()
    {
        return SceneManager.GetActiveScene().name.TryGetEnum<SceneType>();
    }

    public static void LoadScene(SceneType scene, Action<float> callback = null)
    {
        EntryPoint.Mono.StartCoroutine(ILoadScene(scene, callback));
    }

    /// <summary>
    ///     Ищет min x, min y, max x, max y
    /// </summary>
    /// <param name="points">точки</param>
    /// <returns>min x, min y, max x, max y</returns>
    public static (float, float, float, float) MinMax(this Vector2[] points)
    {
        Vector2 minPoint = new() { x = points[0].x, y = points[0].y };
        Vector2 maxPoint = new() { x = points[0].x, y = points[0].y };
        for (int id = 0; id < points.Length; id++)
        {
            if (points[id].x < minPoint.x)
                minPoint.x = points[id].x;

            if (points[id].y < minPoint.y)
                minPoint.y = points[id].y;

            if (points[id].x > maxPoint.x)
                maxPoint.x = points[id].x;

            if (points[id].y > maxPoint.y)
                maxPoint.y = points[id].y;
        }

        return (minPoint.x, minPoint.y, maxPoint.x, maxPoint.y);
    }

    public static void SetColor(this LineRenderer line, Color32 color)
    {
        line.startColor = color;
        line.endColor = color;
    }

    public static bool AnyContains(this string text, params string[] values)
    {
        return values.Any(text.Contains);
    }

    /// <summary>
    ///     Таймер
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
    ///     Получить угол слежения за целью только по оси Y
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
    ///     Следить за целью только по оси Y
    /// </summary>
    /// <param name="transform">следящий</param>
    /// <param name="target">цель</param>
    public static void LookAtY(Transform transform, Vector3 target)
    {
        Vector3 targetPosition = new(target.x, transform.position.y, target.z);
        transform.LookAt(targetPosition);
    }

    /// <summary>
    ///     Следить за целью (2D версия)
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
    ///     Попробовать получить значение другого типа
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
    ///     Находится ли объект в пределах экрана и не перекрыт
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

    public static T RayFromCamera<T>(Vector3? click = null, float z = 100) where T : class
    {
        /*if(!Input.GetMouseButton(0) && Input.touchCount < 1) return null;*/
        Vector3 origin = click ?? MousePointManager.ScreenPosition;
        origin.z -= z;
        Vector3 direction = Vector3.forward;
        if (!Physics.Raycast(origin, direction, out RaycastHit hit)) return null;
        return hit.transform.TryGetComponent(out T obj) ? obj : null;
    }

    /// <summary>
    /// Находится ли объект в пределах экрана
    /// </summary>
    /// <param name="position">объект</param>
    /// <param name="camera">камера</param>
    /// <param name="invert">Если объект позади камеры, инвертируем координаты</param>
    /// <param name="world">Мировые координаты</param>
    /// <param name="screenPos">Позиция объекта на экране</param>
    /// <returns></returns>
    public static bool IsVisibleFrom(this Vector3 position, Camera camera, bool invert, bool world,
        out Vector3 screenPos)
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
    ///     Находится ли объект в пределах экрана
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
    ///     Проверка пересечения с Frustum камеры
    /// </summary>
    /// <param name="renderer">объект</param>
    /// <param name="camera">frustum камера</param>
    /// <returns></returns>
    public static bool FrustumCheck(Renderer renderer, Camera camera)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
        return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
    }

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

    /// <summary>
    ///     Вычисляет позицию 2D камеры, чтобы она была внутри границ
    /// </summary>
    /// <param name="point">Границы, за которые камера не должна выходить (мин, макс)</param>
    /// <param name="c">Камера, над которой производят расчёты</param>
    /// <returns>Позиция камеры внутри границ</returns>
    private static Vector2 InsideBorders(this Camera c, (Vector2 min, Vector2 max) point)
    {
        (Vector2 min, Vector2 max) cameraPoint = CameraBorders(c);
        float size = c.orthographicSize;
        int width = Screen.width;
        int height = Screen.height;
        Vector2 cameraPosition = c.transform.position;
        float wh = (float)width / height;
        float sizeX = size, sizeY = size;
        if ((point.max.x - point.min.x) < (cameraPoint.max.x - cameraPoint.min.x))
            sizeX = (point.max.x - point.min.x) / (2 * wh);
        if ((point.max.y - point.min.y) < (cameraPoint.max.y - cameraPoint.min.y))
            sizeY = (point.max.y - point.min.y) / 2;
        if (!Mathf.Approximately(sizeX, sizeY))
            size = sizeX < sizeY ? sizeX : sizeY;
        c.orthographicSize = size;

        if (point.min.x > cameraPoint.min.x)
            cameraPosition = cameraPosition.SetX(point.min.x + size * wh);
        if (point.min.y > cameraPoint.min.y)
            cameraPosition = cameraPosition.SetY(point.min.y + size);

        if (point.max.x < cameraPoint.max.x)
            cameraPosition = cameraPosition.SetX(point.max.x - size * wh);
        if (point.max.y < cameraPoint.max.y)
            cameraPosition = cameraPosition.SetY(point.max.y - size);
        return cameraPosition;
    }

    /// <summary>
    ///     Вычисляет расположения углов камеры в пространстве
    /// </summary>
    /// <param name="camera">Камера, над которой производят расчёты</param>
    /// <returns>Расположение углов камеры в пространстве</returns>
    private static (Vector2, Vector2) CameraBorders(Camera camera)
    {
        float size = camera.orthographicSize;
        int width = Screen.width;
        int height = Screen.height;
        Vector2 cameraPosition = camera.transform.position;
        Vector2 minCameraPoint,
            maxCameraPoint;
        float wh = (float)width / height;

        minCameraPoint.x = (-size * wh) + cameraPosition.x;
        minCameraPoint.y = -size + cameraPosition.y;
        maxCameraPoint.x = (size * wh) + cameraPosition.x;
        maxCameraPoint.y = size + cameraPosition.y;

        return (minCameraPoint, maxCameraPoint);
    }
}