using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PauseManager;
using Object = UnityEngine.Object;

// ReSharper disable once ClassNeverInstantiated.Global
public class PoolStack<T> where T : MonoBehaviour, IPooling<T>
{
    private readonly Transform PARENT;
    private readonly T PREFAB;
    private readonly Stack<T> STACK = new();
    private Action<T> _build;

    public PoolStack(T prefab, Transform parent = null, int initialCapacity = 0, Action<T> build = null)
    {
        PREFAB = prefab;
        PARENT = parent;
        Build(initialCapacity, build);
    }

    public event Action OnClear, OnDestroy;

    private void Build(int count, Action<T> build)
    {
        _build = build;
        if (count <= 0) return;
        for (int i = 0; i < count; i++)
        {
            T instance = Object.Instantiate(PREFAB, PARENT);
            build?.Invoke(instance);
            instance.SetPooling(this);
            Hide(instance);
        }
    }

    private T Create()
    {
        T instance = Object.Instantiate(PREFAB, PARENT);
        _build?.Invoke(instance);
        instance.SetPooling(this);
        return instance;
    }

    public T Show(bool local = false, float time = 0)
    {
        return Show(Vector3.zero, Quaternion.identity, local, time);
    }

    public T Show(Vector3 position, bool local = false, float time = 0)
    {
        return Show(position, Quaternion.identity, local, time);
    }

    public T Show(Vector3 position, Quaternion rotation, bool local = false, float time = 0)
    {
        T item = STACK.Count > 0 ? STACK.Pop() : Create();
        item.Obj.SetActive(true);
        if (local) item.Tr.SetLocalPositionAndRotation(position, rotation);
        else item.Tr.SetPositionAndRotation(position, rotation);

        if (time == 0) return item;
        if (item.HideCoroutine != null)
            EntryPoint.Mono.StopCoroutine(item.HideCoroutine);
        item.HideCoroutine = EntryPoint.Mono.StartCoroutine(TimerHideCoroutine(item, time));

        return item;
    }

    private IEnumerator TimerHideCoroutine(T item, float time)
    {
        yield return WaitForPausedSeconds(time);
        Hide(item);
    }

    public void Hide(T item)
    {
        if (!item) return;
        item.Obj.SetActive(false);
        STACK.Push(item);
    }

    public void Destroy()
    {
        OnDestroy?.Invoke();
        STACK.Clear();
    }

    public void HideAll()
    {
        OnClear?.Invoke();
    }
}