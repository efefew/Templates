using System;

using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    public static event Action OnEndBootstrap;
    [SerializeField]
    private BootstrapElement[] _bootstrapElements;
    private void Start()
    {
        for (int id = 0; id < _bootstrapElements.Length; id++)
        {
            _bootstrapElements[id].StartBootstrap();
        }

        OnEndBootstrap?.Invoke();
    }
}
