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
    private static T Load<T>() where T : class, new()
    {
        string key = typeof(T).ToString();
        T save = JsonUtility.FromJson<T>(PlayerPrefs.GetString(key)) ?? new T();
        return save;
    }
    private static T Load<T>(string key)
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

    public static void Save<T>(T save) where T : class
    {
        string key = typeof(T).ToString();
        PlayerPrefs.SetString(key, JsonUtility.ToJson(save));
    }
}

#region ControllerData

[Serializable]
public class CameraControllerData
{
}

[Serializable]
public class PlayerControllerData
{
}

#endregion ControllerData

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