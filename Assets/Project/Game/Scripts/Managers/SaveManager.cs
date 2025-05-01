using System;
using System.Collections;

using UnityEngine;
//DontDestroyOnLoad(this);
public static class SaveManager
{
    public static SettingData SettingData { get; private set; }
    public static PlayerData PlayerData { get; private set; }
    public static void DeleteAllSave() => PlayerPrefs.DeleteAll();

    private static T Load<T>(string key)
    {
        T save = JsonUtility.FromJson<T>(PlayerPrefs.GetString(key));
        return save;
    }

    public static IEnumerator LoadAll()
    {
        SettingData = Load<SettingData>(typeof(SettingData).ToString()) ?? new SettingData();
        PlayerData = Load<PlayerData>(typeof(PlayerData).ToString()) ?? new PlayerData();
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
    public float Music;
    public float Sound;
}
[Serializable]
public class PlayerData
{
}