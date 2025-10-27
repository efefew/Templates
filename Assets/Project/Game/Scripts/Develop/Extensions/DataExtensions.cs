#region

using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Object = UnityEngine.Object;

#endregion

public static class DataExtensions
{
    private static IEnumerator DownloadDataCoroutine<T>(string url, Action<T> callbackOnSuccess,
        Action<string> callbackOnError = null, bool removeTrashSymbols = false, bool wrapper = false,
        [CanBeNull] string nameKey = null, [CanBeNull] string valueKey = null)
    {
        url = url.Replace("http://", "https://");
        using UnityWebRequest request = UnityWebRequest.Get(url);

        if (nameKey != null) request.SetRequestHeader(nameKey, valueKey);
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

    private static IEnumerator DownloadImageCoroutine(string url, Image image, Action<string> callbackOnError = null)
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

    private static IEnumerator DownloadTextureCoroutine(string url, Action<Texture2D> callbackOnSuccess,
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

    public static void DownloadData<T>(string url, Action<T> callbackOnSuccess, Action<string> callbackOnError = null,
        bool removeTrashSymbols = false, bool wrapper = false,
        [CanBeNull] string nameKey = null, [CanBeNull] string valueKey = null)
    {
        EntryPoint.Mono.StartCoroutine(
            DownloadDataCoroutine(url, callbackOnSuccess, callbackOnError, removeTrashSymbols, wrapper, nameKey, valueKey));
    }
    public static void DownloadImage(string url, Image image, Action<string> callbackOnError = null)
    {
        EntryPoint.Mono.StartCoroutine(DownloadImageCoroutine(url, image, callbackOnError));
    }

    public static void DownloadTexture(string url, Action<Texture2D> callbackOnSuccess,
        Action<string> callbackOnError = null)
    {
        EntryPoint.Mono.StartCoroutine(DownloadTextureCoroutine(url, callbackOnSuccess, callbackOnError));
    }
    
    public static Texture2D ToTexture2D(this byte[] bytes)
    {
        Texture2D texture = new(2, 2);
        _ = texture.LoadImage(bytes);
        texture.Apply();
        return texture;
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public static Sprite ToSprite(this Texture2D tex) => Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height),
        new Vector2(0.5f, 0.5f), 100.0f);
    public static void Load(this Texture2D texture, string key, bool changeSize = true)
    {
        TextureData data = SaveManager.LoadOrNull<TextureData>(key);
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