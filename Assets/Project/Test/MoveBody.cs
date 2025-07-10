using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class MoveBody : MonoBehaviour
{
    private const float SCALE_MOVE_SPEED = 100000f, SCALE_ROTATE_SPEED = 2000000f;
    private Rigidbody2D _rb;
    private Transform _transform;
    [field: SerializeField] public float ScaleMove { get; private set; } = 1f;
    [field: SerializeField] public float ScaleRotate { get; private set; } = 1f;
    private void Start()
    {
        _transform = transform;
        _rb = GetComponent<Rigidbody2D>();
    }

    public void Move(float speed)
    {
        _rb.AddForce(_transform.up * ScaleMove * speed * SCALE_MOVE_SPEED);
    }
    public void Rotate(float speed)
    {
        _rb.AddTorque(-speed * ScaleRotate * SCALE_ROTATE_SPEED, ForceMode2D.Force);
    }
}
