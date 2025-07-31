#region

using UnityEngine;
using static PauseManager;

#endregion

public class PauseController : MonoBehaviour
{

    private void Update()
    {
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if(IsPause)
                Resume();
            else
                Pause();
        }
        #endif
        if (IsPause)
        {
            return;
        }

        for (int id = 0; id < PausedObjects.Count; id++)
        {
            PausedObjects[id].UpdatePause();
        }
    }

    private void FixedUpdate()
    {
        if (IsPause)
        {
            return;
        }

        /*PauseNewManager.PausedObjects.Add(new PauseController());*/
        for (int id = 0; id < PausedObjects.Count; id++)
        {
            PausedObjects[id].FixedUpdatePause();
        }
    }

    private void LateUpdate()
    {
        if (IsPause)
        {
            return;
        }

        for (int id = 0; id < PausedObjects.Count; id++)
        {
            PausedObjects[id].LateUpdatePause();
        }
    }
}