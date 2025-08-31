using UnityEngine;

using static Key;
using static KeyComputer;
public class ComputerCameraController : CameraController
{
    public ComputerCameraController()
    {
        _zoomIn = new ListKeyCombination(new KeyComputer(TypeSmooth.ScrollUp));
        _zoomOut = new ListKeyCombination(new KeyComputer(TypeSmooth.ScrollDown));

        _rotateClockwise = new ListKeyCombination(new KeyComputer(TypeClick.Hold, KeyCode.E));
        _rotateCounterclockwise = new ListKeyCombination(new KeyComputer(TypeClick.Hold, KeyCode.Q));

        const float SCALE = 5;

        _moveUp = new ListKeyCombination(new KeyCombination(new KeyComputer(TypeClick.Hold, KeyCode.Mouse0), new KeyComputer(TypeSmooth.MouseUp, SCALE)),
                      new KeyCombination(new KeyComputer(TypeClick.Hold, KeyCode.W)),
                      new KeyCombination(new KeyComputer(TypeClick.Hold, KeyCode.UpArrow)));

        _moveDown = new ListKeyCombination(new KeyCombination(new KeyComputer(TypeClick.Hold, KeyCode.Mouse0), new KeyComputer(TypeSmooth.MouseDown, SCALE)),
                      new KeyCombination(new KeyComputer(TypeClick.Hold, KeyCode.S)),
                      new KeyCombination(new KeyComputer(TypeClick.Hold, KeyCode.DownArrow)));

        _moveRight = new ListKeyCombination(new KeyCombination(new KeyComputer(TypeClick.Hold, KeyCode.Mouse0), new KeyComputer(TypeSmooth.MouseRight, SCALE)),
                      new KeyCombination(new KeyComputer(TypeClick.Hold, KeyCode.D)),
                      new KeyCombination(new KeyComputer(TypeClick.Hold, KeyCode.RightArrow)));

        _moveLeft = new ListKeyCombination(new KeyCombination(new KeyComputer(TypeClick.Hold, KeyCode.Mouse0), new KeyComputer(TypeSmooth.MouseLeft, SCALE)),
                      new KeyCombination(new KeyComputer(TypeClick.Hold, KeyCode.A)),
                      new KeyCombination(new KeyComputer(TypeClick.Hold, KeyCode.LeftArrow)));

        _reset = new ListKeyCombination(new KeyComputer(TypeClick.Down, KeyCode.F));
    }
}
