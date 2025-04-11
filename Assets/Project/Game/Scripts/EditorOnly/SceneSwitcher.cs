#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class SceneSwitcherEditor : EditorWindow
{
    private Texture2D[] _rainbowTextures;

    private Color[] _rainbowColors =
    {
        new (0.98f, 0.71f, 0.71f, 1f),
        new (0.98f, 0.83f, 0.71f, 1f),
        new (0.98f, 0.98f, 0.71f, 1f),
        new (0.71f, 0.98f, 0.71f, 1f),
        new (0.71f, 0.98f, 0.98f, 1f),
        new (0.78f, 0.78f, 0.99f, 1f),
        new (0.98f, 0.71f, 0.98f, 1f)
    };
    [MenuItem("Tools/Scenes")]
    public static void ShowWindow()
    {
        GetWindow<SceneSwitcherEditor>("Scenes");
    }
    private void OnGUI()
    {
        GUILayout.Label("сцены", EditorStyles.boldLabel);
        EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;

        for (int i = 0; i < scenes.Length; i++)
        {
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenes[i].path);
            if (GUILayout.Button($"{sceneName}", ButtonStyle(i)))
                EditorSceneManager.OpenScene(scenes[i].path);
        }
    }

    private GUIStyle ButtonStyle(int i)
    {
        // Выбираем цвет из радуги (циклически)
        int colorIndex = i % _rainbowColors.Length;
        Texture2D bgTexture = new (1, 1);
        bgTexture.SetPixel(0, 0, _rainbowColors[colorIndex]);
        bgTexture.Apply();
        Color bgColor = bgTexture.GetPixel(0, 0);
        return new GUIStyle(GUI.skin.button)
        {
            normal = new GUIStyleState()
            {
                background = bgTexture,
                textColor = GetContrastColor(bgColor)
            },
            fontStyle = FontStyle.Bold,
            fontSize = 16
        };
    }

    // Автоматический подбор контрастного цвета текста
    private static Color GetContrastColor(Color bgColor)
    {
        // Рассчитываем яркость фона
        float luminance = 0.2126f * bgColor.r + 
                          0.7152f * bgColor.g + 
                          0.0722f * bgColor.b;
        
        return luminance > 0.6f ? new Color(0.1f, 0.1f, 0.1f, 1f) : new Color(0.9f, 0.9f, 0.9f, 1f);
    }
}
#endif