using UnityEngine;

public abstract class Limit : MonoBehaviour
{
    [SerializeField]
    protected Transform _center;
    protected Transform _tr;

    protected virtual void Awake() => _tr = transform;
    public abstract void UpdateLimit();
    private void Update() => UpdateLimit();
}
