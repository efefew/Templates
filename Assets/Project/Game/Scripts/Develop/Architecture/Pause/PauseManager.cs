using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class PauseManager
{
    public static bool IsPause { get; private set; }
    public static List<IPause> PausedObjects { get; private set; } = new();
    public static event Action OnPause, OnResume;

    [MenuItem("Tools/PauseManager/Pause")]
    public static void Pause()
    {
        IsPause = true;
        OnPause?.Invoke();
    }

    [MenuItem("Tools/PauseManager/Resume")]
    public static void Resume()
    {
        IsPause = false;
        OnResume?.Invoke();
    }

    public static WaitWhile WaitForPausedSeconds(float seconds)
    {
        DateTime endTime = DateTime.Now.AddSeconds(seconds);
        bool isPaused = false;
        WaitWhile waitPausedTime = new(() =>
        {
            DateTime dateTime = DateTime.Now;
            if (isPaused)
            {
                endTime = DateTime.Now.AddSeconds(seconds);
            }
            else if (IsPause && dateTime < endTime)
            {
                isPaused = true;
                seconds = (float)(endTime - dateTime).TotalSeconds;
                return true;
            }

            if (!IsPause) isPaused = false;

            return IsPause || dateTime < endTime;
        });
        return waitPausedTime;
    }
}