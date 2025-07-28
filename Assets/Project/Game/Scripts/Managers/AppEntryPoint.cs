using UnityEngine;
using static UnityExtensions;

public abstract class AppEntryPoint
{
    private static AppEntryPoint _instance;
    public static MonoEntryPoint EntryPoint { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        EntryPoint = new GameObject("EntryPoint").AddComponent<MonoEntryPoint>();
        Object.DontDestroyOnLoad(EntryPoint);
        LoadScene(SceneType.Game);
    }

    public class MonoEntryPoint : MonoBehaviour
    {
    }
}