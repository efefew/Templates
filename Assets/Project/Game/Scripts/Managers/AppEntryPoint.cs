#region

using UnityEngine;
using static UnityExtensions;

#endregion

public abstract class AppEntryPoint
{
    private const SceneType TARGET_SCENE = SceneType.Game;
    private static AppEntryPoint _instance;
    public static MonoEntryPoint EntryPoint { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        EntryPoint = new GameObject("EntryPoint").AddComponent<MonoEntryPoint>();
        Object.DontDestroyOnLoad(EntryPoint);
        if (GetActiveScene() != TARGET_SCENE) LoadScene(TARGET_SCENE);
        else EntryPoint.StartCoroutine(SaveManager.LoadAll());
    }

    public class MonoEntryPoint : MonoBehaviour
    {
    }
}