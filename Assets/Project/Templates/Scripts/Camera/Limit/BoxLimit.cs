using UnityEngine;

public class BoxLimit : Limit
{
    [SerializeField]
    [Min(0)]
    private float _width, _height, _length;
    [SerializeField]
    private bool _widthFree, _heightFree, _lengthFree;
    [SerializeField]
    private bool _local;
    public override void UpdateLimit()
    {
        if (!_center)
        {
            return;
        }

        Quaternion quaternion = _center.rotation;
        if (_local)
        {
            //_center should be parent
            _center.rotation = Quaternion.identity;
        }

        Vector3 center = _center.position;
        if (!_widthFree)
        {
            if (_tr.position.x > center.x + (_width / 2))
            {
                _ = _tr.SetPositionX(center.x + (_width / 2));
            }

            if (_tr.position.x < center.x - (_width / 2))
            {
                _ = _tr.SetPositionX(center.x - (_width / 2));
            }
        }

        if (!_heightFree)
        {
            if (_tr.position.y > center.y + (_height / 2))
            {
                _ = _tr.SetPositionY(center.y + (_height / 2));
            }

            if (_tr.position.y < center.y - (_height / 2))
            {
                _ = _tr.SetPositionY(center.y - (_height / 2));
            }
        }

        if (!_lengthFree)
        {
            if (_tr.position.z > center.z + (_length / 2))
            {
                _ = _tr.SetPositionZ(center.z + (_length / 2));
            }

            if (_tr.position.z < center.z - (_length / 2))
            {
                _ = _tr.SetPositionZ(center.z - (_length / 2));
            }
        }

        if (_local)
        {
            _center.rotation = quaternion;
        }
    }
}

