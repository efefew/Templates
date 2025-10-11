using UnityEngine;
public class FractionUnit : MonoBehaviour
{
    [SerializeField] private Fraction _fraction;
    private Vector3 _spawnPosition;
    public void Build(Fraction fraction)
    {
        _fraction = fraction;
    }

    private void Awake()
    {
        _spawnPosition = transform.position;
    }

    private void OnEnable()
    {
        transform.position = _spawnPosition;
    }
}
