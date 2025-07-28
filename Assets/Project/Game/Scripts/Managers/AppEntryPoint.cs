using UnityEngine;

public abstract class AppEntryPoint
{
    private static MonoEntryPoint _monoEntryPoint;
    private static AppEntryPoint _instance;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        _monoEntryPoint = new GameObject("MonoEntryPoint").AddComponent<MonoEntryPoint>();
        Object.DontDestroyOnLoad(_monoEntryPoint);
        _monoEntryPoint.LoadScene(UnityExtensions.SceneType.Game);
    }

    private class MonoEntryPoint : MonoBehaviour
    {
    }
}