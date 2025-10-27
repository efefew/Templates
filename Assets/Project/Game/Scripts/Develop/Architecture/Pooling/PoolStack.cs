using System;
using System.Collections;
using System.Collections.Generic;
using static PauseManager;
using UnityEngine;
public class PoolStack<T> where T : MonoBehaviour, IPooling<T>
{
    private readonly T _prefab;
    private readonly Stack<T> _stack = new ();
    private readonly Transform _parent;
    public event Action OnClear;
    public PoolStack(T prefab, Transform parent = null)
    {
        _prefab = prefab;
        _parent = parent;
    }

    private T Create(Vector3 position, Quaternion rotation)
    {
        T instance = UnityEngine.Object.Instantiate(_prefab, position, rotation, _parent);
        instance.SetPooling(this);
        _stack.Push(instance);
        return instance;
    }
    
    public T Show(Vector3 position, Quaternion rotation, float time = 0)
    {
        T item = _stack.Count > 0 ? _stack.Pop() : Create(position, rotation);
        item.Obj.SetActive(true);
        item.Tr.position = position;
        item.Tr.rotation = rotation;
        if (time != 0)
        {
            EntryPoint.Mono.StartCoroutine(TimerHideCoroutine(item, time));
        }
        return item;
    }

    private IEnumerator TimerHideCoroutine(T item, float time)
    {
        yield return WaitForPausedSeconds(time);
        Hide(item);
    }
    public void Hide(T item)
    {
        item.Obj.SetActive(false);
        _stack.Push(item);
    }

    public void Clear()
    {
        OnClear?.Invoke();
    }
}