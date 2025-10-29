using System;
using System.Collections;
using System.Collections.Generic;
using static PauseManager;
using UnityEngine;
public class PoolStack<T> where T : MonoBehaviour, IPooling<T>
{
    private readonly T PREFAB;
    private readonly Stack<T> STACK = new ();
    private readonly Transform PARENT;
    public event Action OnClear;
    public PoolStack(T prefab, Transform parent = null)
    {
        PREFAB = prefab;
        PARENT = parent;
    }

    private T Create(Vector3 position, Quaternion rotation)
    {
        T instance = UnityEngine.Object.Instantiate(PREFAB, position, rotation, PARENT);
        instance.SetPooling(this);
        STACK.Push(instance);
        return instance;
    }
    
    public T Show(Vector3 position, Quaternion rotation, float time = 0)
    {
        T item = STACK.Count > 0 ? STACK.Pop() : Create(position, rotation);
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
        STACK.Push(item);
    }

    public void Clear()
    {
        OnClear?.Invoke();
    }
}