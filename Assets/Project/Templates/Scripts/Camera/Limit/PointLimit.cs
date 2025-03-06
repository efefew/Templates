using UnityEngine;


public class PointLimit : Limit
{
    [SerializeField]
    [Min(0)]
    private float _lerp = 0;
    [SerializeField]
    private bool _slow = false;
    public override void UpdateLimit()
    {
        if (!_center)
        {
            return;
        }

        _tr.position = _slow ? Vector3.Lerp(_tr.position, _center.position, _lerp) : _center.position;
    }
}
