using UnityEngine;

public class PoolObject : Pause, ICashed, IPooling<PoolObject>
{
    [field: SerializeField] public Transform Tr { get; private set; }
    [field: SerializeField] public GameObject Obj { get; private set; }
    public PoolStack<PoolObject> Pooling { get; private set; }

    public void SetPooling(PoolStack<PoolObject> pooling)
    {
        Pooling = pooling;
        Pooling.OnClear += Clear;
    }

    private void Clear()
    {
        Pooling.OnClear -= Clear;
        Destroy(Obj);
    }
}
