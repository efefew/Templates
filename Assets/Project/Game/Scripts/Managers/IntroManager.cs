using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    public string Name;
    public void Awake()
    {
        DontDestroyOnLoad(this);
        StartCoroutine(ILoad());
    }

    private IEnumerator ILoad()
    {
        yield return SaveManager.LoadAll();
        AsyncOperation operation = SceneManager.LoadSceneAsync(Name);
        while (operation is { isDone: false })
        {
            // operation.progress;
            yield return null;
        }
    }
}