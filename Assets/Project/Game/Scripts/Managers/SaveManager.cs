using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

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
    public static T Load<T>() where T : class, new()
    {
        string key = typeof(T).ToString();
        return Load<T>(key);
    }
    public static T Load<T>(string key) where T : class, new()
    {
        return LoadOrNull<T>(key) ?? new T();
    }
    public static T LoadOrNull<T>(string key) where T : class
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
    public static void LoadTutorial()
    {
        TutorialData = Load<TutorialData>();
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
    public float Music;
    public float Sound;
    /*private float _music = 1;
    private float _sound = 1;

    [JsonProperty]
    public float Music
    {
        get => _music;
        set
        {
            _music = value;
            SaveManager.Save(this);
        }
    }

    [JsonProperty]
    public float Sound
    {
        get => _sound;
        set
        {
            _sound = value;
            SaveManager.Save(this);
        }
    }*/
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
