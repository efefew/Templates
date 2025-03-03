using System;

using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    public static event Action OnEndBootstrap;
    [Interface(typeof(BootstrapElement))]
    [SerializeField]
    private MonoBehaviour[] _bootstrapElements;

    private void Start()
    {
        for (int id = 0; id < _bootstrapElements.Length; id++)
        {
            (_bootstrapElements[id] as BootstrapElement).StartBootstrap();
        }
#if !UNITY_EDITOR
        _bootstrapElements = null;
#endif
        OnEndBootstrap?.Invoke();
    }
}
