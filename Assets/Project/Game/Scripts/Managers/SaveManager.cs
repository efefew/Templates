using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public static class SaveManager
{
    public static SettingData SettingData { get; private set; }
    public static TutorialData TutorialData { get; private set; }
    public static ControllerData ControllerData { get; private set; }
    public static PlayerData PlayerData { get; private set; }

#if UNITY_EDITOR
    [MenuItem("Tools/SaveManager/DeleteAllSave")]
#endif
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
        return LoadOrNull<T>(key) ?? new T();
    }
    private static T LoadOrNull<T>(string key) where T : class
    {
        return JsonUtility.FromJson<T>(PlayerPrefs.GetString(key));
    }
    public static IEnumerator LoadAll()
    {
        SettingData = Load<SettingData>();
        PlayerData = Load<PlayerData>();
        ControllerData = Load<ControllerData>();
        TutorialData = Load<TutorialData>();
        yield return null;
    }
    public static void Load(this Texture2D texture, string key, bool changeSize = true)
    {
        TextureData data = LoadOrNull<TextureData>(key);
        if (data == null) return;
        bool sizeMatches = texture.width == data.Width && texture.height == data.Height;
        if (!texture || !sizeMatches)
        {
            if (texture && !changeSize) return;
            
            if (!texture) Object.Destroy(texture);
            texture = new Texture2D(data.Width, data.Height, TextureFormat.RGBA32, false)
                { wrapMode = TextureWrapMode.Clamp };
        }

        Color32[] pixels = data.GetPixels();
        texture.SetPixels32(pixels);
        /*_ = texture.LoadImage(data.Pixels);*/
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
public class ControllerData
{
    public PlayerControllerData PlayerData;
    public CameraControllerData CameraData;
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
    /// Устанавливает массив пикселей из Color32[] во внутренний формат RGB.
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
