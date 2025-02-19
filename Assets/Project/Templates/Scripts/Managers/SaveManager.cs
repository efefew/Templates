using System;
using System.Collections;

using UnityEngine;
//DontDestroyOnLoad(this);
public static class SaveManager
{
    public static SettingData SettingData { get; private set; }
    public static void DeleteAllSave() => PlayerPrefs.DeleteAll();

    public static T Load<T>(string key)
    {
        T save = JsonUtility.FromJson<T>(PlayerPrefs.GetString(key));
        return save;
    }

    public static IEnumerator LoadAll()
    {
        SettingData = Load<SettingData>(typeof(SettingData).ToString()) ?? new SettingData();
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
    private bool _music = true;
    private bool _sound = true;

    public bool Music
    { get => _music; set { _music = value; SaveManager.Save(this); } }

    public bool Sound
    { get => _sound; set { _sound = value; SaveManager.Save(this); } }
}