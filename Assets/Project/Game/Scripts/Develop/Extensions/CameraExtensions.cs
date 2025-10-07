using UnityEngine;

public static class CameraExtensions
{
    public static float Wh { get; private set; } = (float)Screen.width / Screen.height;
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

    /// <summary>
    /// Вычисляет расположения углов камеры в пространстве
    /// </summary>
    /// <param name="camera">Камера, над которой производят рассчёты</param>
    /// <param name="cameraTransform"></param>
    /// <returns>Расположение углов камеры в пространстве</returns>
    private static (Vector2, Vector2) CameraBorders(Camera camera, Transform cameraTransform)
    {
        float size = camera.orthographicSize;
        Vector2 cameraPosition = cameraTransform.position;
        Vector2 minCameraPoint,
                maxCameraPoint;

        minCameraPoint.x = (-size * Wh) + cameraPosition.x;
        minCameraPoint.y = -size + cameraPosition.y;
        maxCameraPoint.x = (size * Wh) + cameraPosition.x;
        maxCameraPoint.y = size + cameraPosition.y;

        return (minCameraPoint, maxCameraPoint);
    }

    public static void UpdateWh() => Wh = (float)Screen.width / Screen.height;

    /// <summary>
    /// Вычисляет позицию камеры, чтобы она была внутри границ
    /// </summary>
    /// <param name="cameraTransform"></param>
    /// <param name="point">Границы, за которые камера не должна выходить (мин, макс)</param>
    /// <param name="camera">Камера, над которой производят рассчёты</param>
    /// <param name="z">значение оси z камеры</param>
    /// <returns>Позиция камеры внутри границ</returns>
    public static void InsideBorders(this Camera camera, Transform cameraTransform, (Vector2 min, Vector2 max) point, float z)
    {
        (Vector2 min, Vector2 max) cameraPoint = CameraBorders(camera, cameraTransform);
        Vector2 cameraPosition =  cameraTransform.position;
        float size = SetOrthographicSizeInsideBorders(camera, point, cameraPoint);

        if (point.min.x > cameraPoint.min.x)
            cameraPosition = new Vector2(point.min.x + (size * Wh), cameraPosition.y);
        if (point.min.y > cameraPoint.min.y)
            cameraPosition = new Vector2(cameraPosition.x, point.min.y + size);

        if (point.max.x < cameraPoint.max.x)
            cameraPosition = new Vector2(point.max.x - (size * Wh), cameraPosition.y);
        if (point.max.y < cameraPoint.max.y)
            cameraPosition = new Vector2(cameraPosition.x, point.max.y - size);
        
        cameraTransform.position = new Vector3(cameraPosition.x, cameraPosition.y, z);
    }

    private static float SetOrthographicSizeInsideBorders(Camera camera, (Vector2 min, Vector2 max) point,
        (Vector2 min, Vector2 max) cameraPoint)
    {
        float size = camera.orthographicSize;
        float sizeX = size, sizeY = size;
        
        if ((point.max.x - point.min.x) < (cameraPoint.max.x - cameraPoint.min.x))
            sizeX = (point.max.x - point.min.x) / (2 * Wh);
        
        if ((point.max.y - point.min.y) < (cameraPoint.max.y - cameraPoint.min.y))
            sizeY = (point.max.y - point.min.y) / 2;
        
        if (!Mathf.Approximately(sizeX, sizeY))
            size = sizeX < sizeY ? sizeX : sizeY;
        camera.orthographicSize = size;
        return size;
    }

    /// <summary>
    /// Находится ли объект в пределах экрана и не перекрыт
    /// </summary>
    /// <param name="transform">объект</param>
    /// <param name="camera">камера</param>
    /// <returns></returns>
    public static bool IsRayVisibleFrom(this Camera camera, Transform transform)
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
    public static bool IsVisibleFrom(this Camera camera, Vector3 position, bool invert, bool world,
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
    public static bool IsVisibleFrom(this Camera camera, Vector3 position, bool invert = false, bool world = false)
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
    public static bool FrustumCheck(this Camera camera, Renderer renderer)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
        return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
    }
}
