using UnityEngine;

public class SetDefault : MonoBehaviour
{
    [SerializeField] private Vector3 _defaultPosition;
    [ContextMenu("SetDefaultPosition")]
    public void SetDefaultPosition()
    {
        transform.position = _defaultPosition;
    }
}
