using UnityEngine;
public class IntroManager : MonoBehaviour
{
    [SerializeField]
    private UnityExtensions.SceneType _scene;
    public void Awake()
    {
        DontDestroyOnLoad(this);
        StartCoroutine(UnityExtensions.ILoadScene(_scene));
    }
}