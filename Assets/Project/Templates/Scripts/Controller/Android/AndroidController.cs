public class AndroidController : Controller
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

        CameraController = new AndroidCameraController();
        PlayerController = new AndroidPlayerController();
    }
}
