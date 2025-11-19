using UnityEngine;

public interface IPooling<T> : ICashed where T : MonoBehaviour, IPooling<T>
{
    PoolStack<T> Pooling { get; }
    public Coroutine HideCoroutine { get; set; }
    public void SetPooling(PoolStack<T> pooling);
    public void Clear();
    public void DestroySelf();
}