using UnityEngine;

using static Key;
using static KeyComputer;
public class ComputerCameraController : CameraController
{
    public ComputerCameraController()
    {
        _zoomIn = new(new KeyComputer(TypeSmooth.ScrollUp));
        _zoomOut = new(new KeyComputer(TypeSmooth.ScrollDown));

        _rotateClockwise = new(new KeyComputer(TypeClick.Hold, KeyCode.E));
        _rotateCounterclockwise = new(new KeyComputer(TypeClick.Hold, KeyCode.Q));

        float scale = 5;

        _moveUp = new(new KeyCombination(new KeyComputer(TypeClick.Hold, KeyCode.Mouse0), new KeyComputer(TypeSmooth.MouseUp, scale)),
                      new(new KeyComputer(TypeClick.Hold, KeyCode.W)),
                      new(new KeyComputer(TypeClick.Hold, KeyCode.UpArrow)));

        _moveDown = new(new KeyCombination(new KeyComputer(TypeClick.Hold, KeyCode.Mouse0), new KeyComputer(TypeSmooth.MouseDown, scale)),
                      new(new KeyComputer(TypeClick.Hold, KeyCode.S)),
                      new(new KeyComputer(TypeClick.Hold, KeyCode.DownArrow)));

        _moveRight = new(new KeyCombination(new KeyComputer(TypeClick.Hold, KeyCode.Mouse0), new KeyComputer(TypeSmooth.MouseRight, scale)),
                      new(new KeyComputer(TypeClick.Hold, KeyCode.D)),
                      new(new KeyComputer(TypeClick.Hold, KeyCode.RightArrow)));

        _moveLeft = new(new KeyCombination(new KeyComputer(TypeClick.Hold, KeyCode.Mouse0), new KeyComputer(TypeSmooth.MouseLeft, scale)),
                      new(new KeyComputer(TypeClick.Hold, KeyCode.A)),
                      new(new KeyComputer(TypeClick.Hold, KeyCode.LeftArrow)));

        _reset = new(new KeyComputer(TypeClick.Down, KeyCode.F));
    }
}
