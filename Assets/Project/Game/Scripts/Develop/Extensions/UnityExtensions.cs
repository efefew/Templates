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
        INTRO,
        GAME,
        MENU,
        LOAD,
        LOBBY,
        TEST
    }

    public static void TemporaryVibration(this MonoBehaviour monoBehaviour, XInputController xbox, float lowFrequency,
        float highFrequency, float duration)
    {
        monoBehaviour.StartCoroutine(xbox.ITemporaryVibration(lowFrequency, highFrequency, duration));
    }
    public static T TryGetEnum<T>(this string value) where T : struct, Enum
    {
        if (Enum.TryParse(value, out T result))
            return result;

        throw new ArgumentOutOfRangeException(nameof(value), value, null);
    }
    public static SceneType GetActiveSceneType()
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
    
}