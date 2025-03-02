using System;

using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    public static event Action OnEndBootstrap;
    [RequireInterface(typeof(BootstrapElement))]
    [SerializeField]
    private MonoBehaviour[] _elements;

    private BootstrapElement[] _bootstrapElements;
    private void Awake()
    {
        _bootstrapElements = new BootstrapElement[_elements.Length];
        for (int id = 0; id < _bootstrapElements.Length; id++)
        {
            _bootstrapElements[id] = _elements[id] as BootstrapElement;
        }

        //_elements = null;
    }
    private void Start()
    {
        for (int id = 0; id < _bootstrapElements.Length; id++)
        {
            _bootstrapElements[id].StartBootstrap();
        }

        OnEndBootstrap?.Invoke();
    }
}
