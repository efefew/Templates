using UnityEngine;

public class PoolObject : Pause, ICashed, IPooling<PoolObject>
{
    private GameObject _gameObject;

    private Transform _transform;

    protected override void OnDestroy()
    {
        Pooling.OnDestroy -= DestroySelf;
        Pooling.OnClear -= Clear;
    }

    public Transform Tr
    {
        get
        {
            if (!_transform)
                _transform = transform;
            return _transform;
        }
    }

    public GameObject Obj
    {
        get
        {
            if (!_gameObject)
                _gameObject = gameObject;
            return _gameObject;
        }
    }

    public PoolStack<PoolObject> Pooling { get; private set; }
    public Coroutine HideCoroutine { get; set; }

    public void SetPooling(PoolStack<PoolObject> pooling)
    {
        Pooling = pooling;
        Pooling.OnClear += Clear;
        Pooling.OnDestroy += DestroySelf;
    }

    public void Clear()
    {
        if (HideCoroutine != null)
            StopCoroutine(HideCoroutine);
        Pooling.Hide(this);
    }

    public void DestroySelf()
    {
        Pooling.OnDestroy -= DestroySelf;
        Pooling.OnClear -= Clear;
        Destroy(Obj);
    }
}