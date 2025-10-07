#region

using UnityEditor;
using UnityEngine;
using static UnityExtensions;

#endregion

public abstract class EntryPoint
{
    private const SceneType TARGET_SCENE = SceneType.GAME;
    private static EntryPoint _instance;
    public static MonoEntryPoint Mono { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        /*Application.targetFrameRate = 60;*/
        Mono = new GameObject("EntryPoint").AddComponent<MonoEntryPoint>();
        Object.DontDestroyOnLoad(Mono);
        /*if (GetActiveScene() != TARGET_SCENE) LoadScene(TARGET_SCENE);
        else */Mono.StartCoroutine(SaveManager.LoadAll());
    }
#if UNITY_EDITOR
    [MenuItem("Tools/Restart")]
#endif
    public static void Restart()
    {
        LoadScene(GetActiveSceneType());
    }
    public class MonoEntryPoint : MonoBehaviour
    {
    }
}