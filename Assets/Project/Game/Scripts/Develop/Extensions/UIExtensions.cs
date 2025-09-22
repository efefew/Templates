using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class UIExtensions
{
    /// <summary>
    ///     Проверяет, находится ли указатель мыши над UI-элементом, принадлежащим  Canvas.
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
    ///     Проверяет, находится ли указатель мыши над объектом UI.
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
    private static IEnumerator ForceChangeParentUICoroutine(RectTransform rt, RectTransform newParent)
    {
        if (!rt || !newParent) yield break;

        // Получаем Canvas и Camera для source/target
        Camera sourceCam = GetCanvasFor(rt);
        Camera targetCam = GetCanvasFor(newParent);

        // Временно фиксируем размер через LayoutElement
        LayoutElement tempLe = EnsureLayoutElement(rt, out bool addedLayoutElement);
        ApplyLayoutElementSize(tempLe, rt.rect.size);

        // Отключаем layout компоненты у старого и нового родителя, сохранив их состояния
        RectTransform oldParentRect = rt.parent as RectTransform;
        LayoutGroup[] oldGroups = GetLayoutGroups(oldParentRect, out ContentSizeFitter[] oldFitters);
        LayoutGroup[] newGroups = GetLayoutGroups(newParent, out ContentSizeFitter[] newFitters);

        bool[] oldGroupsEnabled = DisableComponents(oldGroups);
        bool[] oldFittersEnabled = DisableComponents(oldFitters);
        bool[] newGroupsEnabled = DisableComponents(newGroups);
        bool[] newFittersEnabled = DisableComponents(newFitters);
        
        Canvas.ForceUpdateCanvases();
        yield return null;

        // Вычисляем локальные точки и bounds в координатах newParent
        Vector2[] localPoints = ConvertWorldCornersToLocalPoints(rt, sourceCam, newParent, targetCam);
        (Vector2 size, Vector2 centerLocal) = CalculateBounds(localPoints);

        // Меняем родителя и применяем позицию/размер
        ApplyRectTransformToParent(rt, newParent, centerLocal, size);

        Canvas.ForceUpdateCanvases();
        yield return null;

        // Восстанавливаем состояния компонентов
        RestoreComponents(oldGroups, oldGroupsEnabled);
        RestoreComponents(oldFitters, oldFittersEnabled);
        RestoreComponents(newGroups, newGroupsEnabled);
        RestoreComponents(newFitters, newFittersEnabled);

        // Удаляем временный LayoutElement
        if (addedLayoutElement && tempLe)
            UnityEngine.Object.Destroy(tempLe);

        Canvas.ForceUpdateCanvases();
    }
    public static void ForceChangeParentUI(this RectTransform rectTransform, RectTransform newParent)
    {
        EntryPoint.Mono.StartCoroutine(ForceChangeParentUICoroutine(rectTransform, newParent));
    }
    private static Camera GetCanvasFor(RectTransform rt)
    {
        Canvas canvas = rt.GetComponentInParent<Canvas>();
        Camera cam = (canvas && canvas.renderMode == RenderMode.ScreenSpaceCamera) ? canvas.worldCamera : null;
        return cam;
    }

    private static LayoutElement EnsureLayoutElement(RectTransform rt, out bool added)
    {
        added = false;
        if (!rt) return null;
        LayoutElement le = rt.GetComponent<LayoutElement>();
        if (!le)
        {
            le = rt.gameObject.AddComponent<LayoutElement>();
            added = true;
        }

        return le;
    }

    private static void ApplyLayoutElementSize(LayoutElement le, Vector2 size)
    {
        if (!le) return;
        le.preferredWidth = size.x;
        le.preferredHeight = size.y;
        le.flexibleWidth = 0;
        le.flexibleHeight = 0;
    }

    private static LayoutGroup[] GetLayoutGroups(RectTransform parent, out ContentSizeFitter[] fitters)
    {
        if (!parent)
        {
            fitters = Array.Empty<ContentSizeFitter>();
            return Array.Empty<LayoutGroup>();
        }

        fitters = parent.GetComponents<ContentSizeFitter>();
        return parent.GetComponents<LayoutGroup>();
    }

    private static bool[] DisableComponents(Behaviour[] components)
    {
        if (components == null || components.Length == 0) return Array.Empty<bool>();
        bool[] states = new bool[components.Length];
        for (int i = 0; i < components.Length; i++)
        {
            states[i] = components[i].enabled;
            components[i].enabled = false;
        }

        return states;
    }

    private static void RestoreComponents(Behaviour[] components, bool[] states)
    {
        if (components == null || components.Length == 0 || states == null) return;
        for (int i = 0; i < components.Length && i < states.Length; i++)
            components[i].enabled = states[i];
    }

    private static Vector2[] ConvertWorldCornersToLocalPoints(RectTransform rt, Camera sourceCam, RectTransform newParent, Camera targetCam)
    {
        var worldCorners = new Vector3[4];
        rt.GetWorldCorners(worldCorners);
        var screenPoints = new Vector2[4];
        for (int i = 0; i < 4; i++)
            screenPoints[i] = RectTransformUtility.WorldToScreenPoint(sourceCam, worldCorners[i]);

        var localPoints = new Vector2[4];
        for (int i = 0; i < 4; i++)
            RectTransformUtility.ScreenPointToLocalPointInRectangle(newParent, screenPoints[i], targetCam, out localPoints[i]);

        return localPoints;
    }

    private static (Vector2 size, Vector2 center) CalculateBounds(Vector2[] localPoints)
    {
        if (localPoints == null || localPoints.Length == 0) return (Vector2.zero, Vector2.zero);
        float minX = localPoints[0].x, maxX = localPoints[0].x, minY = localPoints[0].y, maxY = localPoints[0].y;
        for (int i = 1; i < localPoints.Length; i++)
        {
            if (localPoints[i].x < minX) minX = localPoints[i].x;
            if (localPoints[i].x > maxX) maxX = localPoints[i].x;
            if (localPoints[i].y < minY) minY = localPoints[i].y;
            if (localPoints[i].y > maxY) maxY = localPoints[i].y;
        }

        var size = new Vector2(maxX - minX, maxY - minY);
        var centerLocal = new Vector2((minX + maxX) * 0.5f, (minY + maxY) * 0.5f);
        return (size, centerLocal);
    }

    private static void ApplyRectTransformToParent(RectTransform rt, RectTransform newParent, Vector2 centerLocal, Vector2 size)
    {
        rt.SetParent(newParent, worldPositionStays: false);
        rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = centerLocal;
        rt.sizeDelta = size;
    }
    private static bool[] DisableComponents(LayoutGroup[] components) => DisableComponents(Array.ConvertAll(components, b => (Behaviour)b));
    private static bool[] DisableComponents(ContentSizeFitter[] components) => DisableComponents(Array.ConvertAll(components, b => (Behaviour)b));
    private static void RestoreComponents(LayoutGroup[] components, bool[] states) => RestoreComponents(Array.ConvertAll(components, b => (Behaviour)b), states);
    private static void RestoreComponents(ContentSizeFitter[] components, bool[] states) => RestoreComponents(Array.ConvertAll(components, b => (Behaviour)b), states);
}
