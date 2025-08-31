using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

public static class SaveManager
{
    public static SettingData SettingData { get; private set; }
    public static TutorialData TutorialData { get; private set; }
    public static PlayerData PlayerData { get; private set; }

    [MenuItem("Tools/SaveManager/DeleteAllSave")]
    public static void DeleteAllSave() => PlayerPrefs.DeleteAll();
    public static void Delete<T>()
    {
        string key = typeof(T).ToString();
        PlayerPrefs.DeleteKey(key);
    }
    private static T Load<T>() where T : class, new()
    {
        string key = typeof(T).ToString();
        return Load<T>(key);
    }
    private static T Load<T>(string key) where T : class, new()
    {
        T save = JsonUtility.FromJson<T>(PlayerPrefs.GetString(key)) ?? new T();
        return save;
    }
    private static T LoadOrNull<T>(string key) where T : class
    {
        T save = JsonUtility.FromJson<T>(PlayerPrefs.GetString(key));
        return save;
    }
    public static IEnumerator LoadAll()
    {
        SettingData = Load<SettingData>();
        PlayerData = Load<PlayerData>();
        TutorialData = Load<TutorialData>();
        yield return null;
    }
    public static void Load(this Texture2D texture, string key, bool changeSize = true)
    {
        TextureData data = LoadOrNull<TextureData>(key);
        if (data == null) return;
        if (!texture || data.Width != texture.width || data.Height != texture.height)
        {
            if (!texture || changeSize)
            {
                texture = new Texture2D(data.Width, data.Height, TextureFormat.RGBA32, false) { wrapMode = TextureWrapMode.Clamp };
            }
            else return;
        }

        Color32[] pixels = data.GetPixels();
        texture.SetPixels32(pixels);
        texture.Apply();
    }
    public static void Save(this Texture2D texture, string key)
    {
        TextureData data = new(texture.width, texture.height);
        data.SetPixels(texture.GetPixels32());
        PlayerPrefs.SetString(key, JsonUtility.ToJson(data));
    }
    public static void Save<T>(T save) where T : class
    {
        string key = typeof(T).ToString();
        PlayerPrefs.SetString(key, JsonUtility.ToJson(save));
    }
}

[Serializable]
public class CameraControllerData
{
}
[Serializable]
public class PlayerControllerData
{
}

[Serializable]
public class SettingData
{
    public float Music = 1;
    public float Sound = 1;
}

[Serializable]
public class PlayerData
{
}
[Serializable]
public class TutorialData
{
    public bool StartTutorialCompleted;
}
[Serializable]
public class TextureData
{
    /// <summary>
    /// Ширина текстуры.
    /// </summary>
    public int Width;

    /// <summary>
    /// Высота текстуры.
    /// </summary>
    public int Height;

    /// <summary>
    /// Массив байтов пикселей (RGB, 3 байта на пиксель).
    /// </summary>
    public byte[] Pixels;

    /// <summary>
    /// Создает новый объект TextureData с заданными размерами.
    /// </summary>
    /// <param name="width">Ширина текстуры.</param>
    /// <param name="height">Высота текстуры.</param>
    public TextureData(int width, int height)
    {
        Width = width;
        Height = height;
        Pixels = new byte[width * height * 3];
    }

    /// <summary>
    /// Устанавливает массив пикселей из Color32[] в внутренний формат RGB.
    /// </summary>
    /// <param name="colors">Массив цветов пикселей.</param>
    public void SetPixels(Color32[] colors)
    {
        for (int i = 0; i < colors.Length; i++)
        {
            int idx = i * 3;
            Pixels[idx + 0] = colors[i].r;
            Pixels[idx + 1] = colors[i].g;
            Pixels[idx + 2] = colors[i].b;
        }
    }

    /// <summary>
    /// Получает массив пикселей в формате Color32 из внутреннего массива байтов.
    /// </summary>
    /// <returns>Массив пикселей Color32.</returns>
    public Color32[] GetPixels()
    {
        var result = new Color32[Width * Height];
        for (int i = 0; i < result.Length; i++)
        {
            int idx = i * 3;
            result[i] = new Color32(
                Pixels[idx + 0],
                Pixels[idx + 1],
                Pixels[idx + 2],
                255
            );
        }
        return result;
    }
}
