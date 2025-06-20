using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
public class RectCollider : MonoBehaviour
{
    [SerializeField]
    private bool _offsetX, _offsetY;
    private RectTransform _targetRect;
    private BoxCollider2D _boxCollider;

    private void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _targetRect = transform as RectTransform;
    }

    private void Update()
    {
        AdjustCollider();
    }
    private void AdjustCollider()
    {
        if (!_targetRect) return;
        Vector2 size = _targetRect.rect.size;
        _boxCollider.size = new Vector2(size.x, size.y);
        _boxCollider.offset = new Vector2(_offsetX ? -size.x / 2 : _boxCollider.offset.x, _offsetY ? -size.y / 2 : _boxCollider.offset.y);
    }
}

