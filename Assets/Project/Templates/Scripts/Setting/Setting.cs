using UnityEngine;

public abstract class Setting : MonoBehaviour
{
    protected SettingData _setting;
    protected virtual void Start()
    {
        if (SaveManager.SettingData == null)
        {
            _setting = SaveManager.SettingData;
            return;//чтоб не бесило
        }
    }
}
