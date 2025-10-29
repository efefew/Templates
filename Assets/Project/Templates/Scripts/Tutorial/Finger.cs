using UnityEngine;

public class Finger : MonoBehaviour
{
    public enum DirectionFingerType
    {
        Right = 0,
        Left = 180,
        Up = 90,
        Down = 270
    }

    [SerializeField] private Transform _finger;
    [SerializeField] private RectTransform _fingerTutorial;

    public void SetPoint(RectTransform point, DirectionFingerType direction)
    {
        _finger.gameObject.SetActive(true);
        _fingerTutorial.position = point.position;
        _finger.rotation = Quaternion.Euler(Vector3.zero.AddZ((float)direction));
    }

    public void Hide()
    {
        _finger.gameObject.SetActive(false);
    }
}