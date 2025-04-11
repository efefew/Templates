using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class ScrollReaderAPI<TData, TBlock> : ReaderAPI where TBlock : class
{
    /// <summary>
    /// Порог прокрутки
    /// </summary>
    private const float SCROLL_THRESHOLD = 0.15f;
    protected Action _onLoadData;
    [SerializeField]
    protected ScrollRect _scroll;
    [SerializeField]
    protected LayoutGroup _content;
    private Coroutine _coroutine;
    protected int _id;
    protected bool _isLoading;
    [SerializeField] protected TBlock _blockPrefab;
    protected TData _data;
    protected virtual void Awake()
    {
        _scroll.onValueChanged.AddListener(OnScroll);
    }
    protected virtual void OnEnable()
    {
        StartData();
    }
    private void OnDisable()
    {
        _data = default;
        _isLoading = false;
    }
    protected virtual void OnDestroy()
    {
        _scroll.onValueChanged.RemoveListener(OnScroll);
    }
    private void OnScroll(Vector2 value)
    {
        if (_scroll.verticalNormalizedPosition >= SCROLL_THRESHOLD) return;
        AppendData();
    }
    protected virtual Action<TData> CallbackOnSuccess(TBlock block)
    {
        return newsData =>
        {
            _data = newsData;
            _isLoading = false;
            _onLoadData?.Invoke();
            _onLoadData = null;
            if(block != null) Build(block);
        };
    }
    protected virtual void Build(TBlock block)
    {
        if (_id >= GetCountDataElements(_data))
        {
            _onLoadData += () => Build(block);
            return;
        }

        if (block == null) return;
        BuildBlock(_data, _id);
        _id++;
    }
    protected abstract int GetCountDataElements(TData data);
    protected abstract void BuildBlock(TData data, int index);
    public override void ClearData()
    {
        _content.transform.Clear();
    }
    public override void StartData()
    {
        _id = 0;
        ClearData();
        if(_coroutine != null) StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(IUpdateData());
    }
    private IEnumerator IUpdateData()
    {
        yield return new WaitForFixedUpdate();
        _scroll.verticalNormalizedPosition = 0;
        // ReSharper disable once CompareOfFloatsByEqualityOperator
        while (_scroll.verticalNormalizedPosition is 1f or < SCROLL_THRESHOLD)
        {
            _scroll.verticalNormalizedPosition = 0;
            AppendData();
            yield return new WaitForFixedUpdate();
        }
        _scroll.verticalNormalizedPosition = 1;
    }
}