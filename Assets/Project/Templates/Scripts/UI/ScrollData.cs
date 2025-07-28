using UnityEngine;
using UnityEngine.UI;
using static UnityExtensions;

public abstract class ScrollData<T> : MonoBehaviour
{
    private const float SCROLL_THRESHOLD = 0.15f;
    [SerializeField] protected ScrollRect _scroll;
    [SerializeField] protected Transform _content;
    [SerializeField] protected string _url;
    [SerializeField] private bool _wrapper, _removeTrashSymbols;
    [SerializeField] private int _scrollAmount = 2;
    protected T _data;
    protected int _id = 0;

    protected virtual void Start()
    {
        _scroll.onValueChanged.AddListener(call => OnScroll());
        DownloadData<T>(_url, OnDownloadData, wrapper: _wrapper, removeTrashSymbols: _removeTrashSymbols);
    }

    protected virtual void OnEnable()
    {
        if (_data == null) return;
        for (int id = 0; id < _scrollAmount; id++)
        {
            OnScroll(false);
        }
    }

    protected virtual void OnDisable()
    {
        _content.Clear();
        _scroll.verticalNormalizedPosition = 0;
    }

    protected virtual void OnDownloadData(T data)
    {
        _data = data;
        for (int id = 0; id < _scrollAmount; id++)
        {
            OnScroll(false);
        }
    }

    protected abstract void CreateDataObject();

    protected virtual bool OnScroll(bool scroll = true)
    {
        if (_data == null) return false;
        if (scroll && _scroll.verticalNormalizedPosition >= SCROLL_THRESHOLD) return false;
        CreateDataObject();
        return true;
    }
}