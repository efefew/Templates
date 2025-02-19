public class ComputerController : Controller
{
    public override void StartBootstrap()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }

        CameraController = new ComputerCameraController();
        PlayerController = new ComputerPlayerController();
    }
}
