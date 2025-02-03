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