using UnityEngine;

public abstract class Controller : MonoBehaviour, BootstrapElement
{
    public static Controller Instance { get; protected set; }
    [field: SerializeField]
    public CameraController CameraController { get; protected set; }
    [field: SerializeField]
    public PlayerController PlayerController { get; protected set; }

    public abstract void StartBootstrap();
}
