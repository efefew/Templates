using System.Collections.Generic;
using UnityEditor;

public static class PauseManager
{
    public static bool IsPause {get; private set;}
    public static List<IPause> PausedObjects { get; private set; } = new();
    
    
    [MenuItem("Tools/PauseManager/Pause")]
    public static void Pause()
    {
        IsPause = true;
    }
    [MenuItem("Tools/PauseManager/Resume")]
    public static void Resume()
    {
        IsPause = false;
    }
}
