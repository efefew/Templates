using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.XInput;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static CoroutineExtensions;
using static UnityExtensions;
using Object = UnityEngine.Object;

public static class CoroutineExtensions
{
    private static int _countTemporaryVibration;

    public static IEnumerator ILoadScene(SceneType scene, Action<float> callback = null)
    {
        yield return SaveManager.LoadAll();
        AsyncOperation operation = SceneManager.LoadSceneAsync(GetNameScenes(scene));
        while (operation is { isDone: false })
        {
            callback?.Invoke(operation.progress);
            yield return null;
        }
    }

    public static IEnumerator IDownloadData<T>(string url, Action<T> callbackOnSuccess,
        Action<string> callbackOnError = null, bool removeTrashSymbols = false, bool wrapper = false)
    {
        url = url.Replace("http://", "https://");
        using UnityWebRequest request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            if (wrapper)
                json = "{\"items\":" + json + "}";
            if (removeTrashSymbols)
                json = json.Replace("$", "");
            T component = JsonUtility.FromJson<T>(json);
            callbackOnSuccess?.Invoke(component);
        }
        else
            callbackOnError?.Invoke(request.error);
    }

    public static IEnumerator IDownloadImage(string url, Image image, Action<string> callbackOnError = null)
    {
        url = url.Replace("http://", "https://");
        using UnityWebRequest request = UnityWebRequestTexture.GetTexture(new Uri(url));
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            if (image) image.sprite = DownloadHandlerTexture.GetContent(request).ToSprite();
        }
        else
            callbackOnError?.Invoke(request.error);
    }

    public static IEnumerator IDownloadTexture(string url, Action<Texture2D> callbackOnSuccess,
        Action<string> callbackOnError = null)
    {
        url = url.Replace("http://", "https://");
        using UnityWebRequest request = UnityWebRequestTexture.GetTexture(new Uri(url));
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
            callbackOnSuccess(DownloadHandlerTexture.GetContent(request));
        else
            callbackOnError?.Invoke(request.error);
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

    public static string GetNameScenes(SceneType scene)
    {
        return scene switch
        {
            SceneType.Intro => "Intro",
            /*SceneType.Menu => "Menu",
            SceneType.Load => "Load",
            SceneType.Lobby => "Lobby",*/
            SceneType.Game => "Game",
            _ => ""
        };
    }

    public static void LoadScene(this MonoBehaviour monoBehaviour, SceneType scene, Action<float> callback = null)
    {
        monoBehaviour.StartCoroutine(ILoadScene(scene, callback));
    }

    public static void DownloadData<T>(this MonoBehaviour monoBehaviour, string url, Action<T> callbackOnSuccess,
        Action<string> callbackOnError = null,
        bool removeTrashSymbols = false, bool wrapper = false)
    {
        monoBehaviour.StartCoroutine(
            IDownloadData(url, callbackOnSuccess, callbackOnError, removeTrashSymbols, wrapper));
    }

    public static void DownloadImage(this MonoBehaviour monoBehaviour, string url, Image image,
        Action<string> callbackOnError = null)
    {
        monoBehaviour.StartCoroutine(IDownloadImage(url, image, callbackOnError));
    }

    public static void DownloadTexture(this MonoBehaviour monoBehaviour, string url,
        Action<Texture2D> callbackOnSuccess, Action<string> callbackOnError = null)
    {
        monoBehaviour.StartCoroutine(IDownloadTexture(url, callbackOnSuccess, callbackOnError));
    }


    /// <summary>
    /// Ищет min x, min y, max x, max y
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

    public static Texture2D ToTexture2D(this byte[] bytes)
    {
        Texture2D texture = new(2, 2);
        _ = texture.LoadImage(bytes);
        texture.Apply();
        return texture;
    }

    public static Sprite ToSprite(this Texture2D tex) => Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height),
        new Vector2(0.5f, 0.5f), 100.0f);

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
    /// Проверяет, находится ли указатель мыши над UI-элементом, принадлежащим  Canvas.
    /// </summary>
    /// <param name="canvas">Canvas, над которым нужно проверить указатель.</param>
    /// <returns>true, если указатель над UI, false иначе.</returns>
    public static bool IsPointerOverUI(Canvas canvas)
    {
        if (!EventSystem.current) return false;

        GraphicRaycaster gr = canvas.GetComponent<GraphicRaycaster>();
        if (!gr) return false;

        PointerEventData data = new(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new();
        gr.Raycast(data, results);
        return results.Count > 0;
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
    ///  Находится ли объект в пределах экрана
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
}