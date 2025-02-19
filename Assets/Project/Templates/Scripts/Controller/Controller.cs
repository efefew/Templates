public abstract class Controller : BootstrapElement
{
    public static Controller Instance { get; protected set; }
    public CameraController CameraController { get; protected set; }
    public PlayerController PlayerController { get; protected set; }
}
