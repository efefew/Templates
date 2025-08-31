using UnityEngine;

public interface IPooling<T> : ICashed where T : MonoBehaviour, IPooling<T>
{
    PoolStack<T> Pooling{ get;}
    public void SetPooling(PoolStack<T> pooling);
}
