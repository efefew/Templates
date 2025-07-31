#region

using UnityEngine;
using static UnityExtensions;

#endregion

public abstract class EntryPoint
{
    private const SceneType TARGET_SCENE = SceneType.Game;
    private static EntryPoint _instance;
    public static MonoEntryPoint Mono { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        Mono = new GameObject("EntryPoint").AddComponent<MonoEntryPoint>();
        Object.DontDestroyOnLoad(Mono);
        if (GetActiveScene() != TARGET_SCENE) LoadScene(TARGET_SCENE);
        else Mono.StartCoroutine(SaveManager.LoadAll());
    }

    public class MonoEntryPoint : MonoBehaviour
    {
    }
}