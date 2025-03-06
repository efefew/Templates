using UnityEngine;

public class CircleLimit : Limit
{
    [SerializeField]
    [Min(0)]
    private float _radius = 1f;
    [SerializeField]
    protected bool _local;
    protected override void Awake()
    {
        base.Awake();
        _tr.localEulerAngles = Vector3.zero;
    }
    public override void UpdateLimit()
    {
        float distance = _local ? Vector2.Distance(_tr.localPosition, _center.localPosition) : Vector2.Distance(_tr.position, _center.position);
        if (!_center || distance <= _radius)
        {
            return;
        }

        float positionZ = _local ? _tr.localPosition.z : _tr.position.z;
        float angleZ = _local ? _tr.localEulerAngles.z : _tr.eulerAngles.z;
        SetLimit(_tr);
        _ = _tr.SetPositionZ(positionZ, _local).SetAngleZ(angleZ, _local);
    }
    private void SetLimit(Transform tr)
    {
        Quaternion quaternion = _center.rotation;
        if (_local)
        {
            //_center should be parenting
            _center.rotation = Quaternion.identity;
        }

        Vector3 relative = tr.InverseTransformPoint(_center.position);
        tr.position = _center.position;
        tr.eulerAngles = new Vector3(0, 0, -Mathf.Atan2(relative.x, relative.y) * Mathf.Rad2Deg);
        tr.position -= tr.up * _radius;

        if (_local)
        {
            _center.rotation = quaternion;
        }
    }
}

