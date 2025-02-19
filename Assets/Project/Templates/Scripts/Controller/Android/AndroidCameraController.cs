using static KeyAndroid;

public class AndroidCameraController : CameraController
{
    public AndroidCameraController()
    {
        _zoomIn = new(new KeyAndroid(TypeSmooth.ZoomIn));
        _zoomOut = new(new KeyAndroid(TypeSmooth.ZoomOut));

        _rotateClockwise = new(new KeyAndroid(TypeSmooth.RotateClockwise));
        _rotateCounterclockwise = new(new KeyAndroid(TypeSmooth.RotateCounterclockwise));

        _moveUp = new(new KeyAndroid(TypeSmooth.MoveUp));
        _moveDown = new(new KeyAndroid(TypeSmooth.MoveDown));

        _moveRight = new(new KeyAndroid(TypeSmooth.MoveRight));
        _moveLeft = new(new KeyAndroid(TypeSmooth.MoveLeft));
        //_reset
    }
}
