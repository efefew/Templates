using UnityEngine;

public class Singleton<T>: MonoBehaviour where T : Singleton<T>
{
    private static T _cachedInstance;
    public static T Instance
    {
        get
        {
            if (!_cachedInstance)
            {
                _cachedInstance = FindAnyObjectByType<T>();

                if (!_cachedInstance)
                {
                    Debug.LogWarning($"There is no instance of {typeof(T).Name}");
                    return null;
                }
				
                /*DontDestroyOnLoad(_cachedInstance.gameObject);*/
            }

            return _cachedInstance;
        }
    }
}
