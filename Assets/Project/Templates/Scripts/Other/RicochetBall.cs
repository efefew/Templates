using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
[RequireComponent(typeof(Rigidbody2D))]
public class RicochetBall : MonoBehaviour
{
    [SerializeField]
    private float _speed = 100;

    private Rigidbody2D _rb;
    [SerializeField]
    private Vector2 _direction;
    private Vector2 _lastVelocity;
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _direction = Random.insideUnitCircle.normalized;
    }

    private void FixedUpdate()
    {
        _rb.linearVelocity = _direction * _speed;
        _lastVelocity = _rb.linearVelocity;
    }
    public void ChangeSpeed(float scale)
    {
        _speed *= scale;
        _rb.linearVelocity *= _speed;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contactCount <= 0) return;
        Vector2 averageNormal = collision.contacts.Aggregate(Vector2.zero, (current, contact) => current + contact.normal);
        Vector2 normal = (averageNormal / collision.contactCount).normalized;
        _direction = Vector2.Reflect(_lastVelocity.normalized, normal);
    }
}
