using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Slider))]
public class SliderBarUI : MonoBehaviour
{
    [SerializeField] private Color _color;
    [SerializeField] private Image _fill, _fillPoint, _background, _border;
    [SerializeField] private RectTransform _tempFill;
    [SerializeField] private float _lerpTempFill = 0.1f;
    [SerializeField] private float _timeTempFill = 1f;
    [SerializeField] private float _minDelta = 1f;

    private StateBar _stateBar;
    private Slider _slider;
    /*private RectTransform _rectTransform;*/
    private Animation _animation;
    private float _maxWidth, _width;
    private Coroutine _coroutineChangeTempFill;
    private DateTime _lastChange;
    private void Awake()
    {
        /*_rectTransform = GetComponent<RectTransform>();*/
        _slider = GetComponent<Slider>();
        TryGetComponent(out _animation);
    }

    private void OnEnable()
    {
        _fill.color = _color;
        _fillPoint.color = _color;
        
        Color.RGBToHSV(_color, out float h, out float s, out float v);
        /*Color backgroundColor = Color.HSVToRGB(h, s, v / 4f);
        backgroundColor.a = 0.9f;*/
        _background.color = Color.HSVToRGB(h, s, 0.1f);
        /*_border.color = Color.HSVToRGB(h, s, 0.1f);*/
        _border.color = new Color(0.05f, 0.05f, 0.05f, 1);
    }
    public void Build(StateBar stateBar)
    {
        _stateBar = stateBar;
        OnChangedValue(_stateBar);
        _stateBar.OnChanged += OnChangedValue;
        _stateBar.OnEmpty += OnEmptyValue;
    }
    private void OnDestroy()
    {
        _stateBar.OnChanged -= OnChangedValue;
        _stateBar.OnEmpty -= OnEmptyValue;
    }
    private void OnChangedValue(StateBar stateBar)
    {
        /*_slider.maxValue = stateBar.MaxValue;
        _maxWidth = Mathf.Max(65, stateBar.MaxValue * 5);
        _rectTransform.sizeDelta = _rectTransform.sizeDelta.SetX(_maxWidth);
        _slider.value = stateBar.Value;*/
        _slider.maxValue = 1;
        if ((stateBar.Value / stateBar.MaxValue).TryConvertToFloat(out float f))
            _slider.value = f;
        _animation.Play("On Take Damage");
        
        if(!_tempFill) return;
        _lastChange = DateTime.Now.AddSeconds(_timeTempFill);
        _coroutineChangeTempFill ??= StartCoroutine(IChangeTempFill());
    }

    private IEnumerator IChangeTempFill()
    {
        yield return new WaitWhile(() => DateTime.Now < _lastChange);
        while (Mathf.Abs(_maxWidth + _tempFill.sizeDelta.x) - Mathf.Abs(GetWidth()) > _minDelta)
        {
            yield return new WaitForEndOfFrame();
            float width = GetWidth();
            SetTempWidth(Mathf.Lerp(_maxWidth + _tempFill.sizeDelta.x, width, _lerpTempFill));
        }
        SetTempWidth(GetWidth());
        _coroutineChangeTempFill = null;
    }
    private void SetTempWidth(float width)
    {
        _tempFill.sizeDelta = _tempFill.sizeDelta.SetX(-(_maxWidth - width));
        _tempFill.anchoredPosition = _tempFill.anchoredPosition.SetX(_tempFill.sizeDelta.x / 2f);
    }
    private float GetWidth()
    {
        return _maxWidth * _slider.value / _slider.maxValue;
    }

    private void OnEmptyValue(StateBar stateBar)
    {
        _fillPoint.gameObject.SetActive(false);
        if(!_stateBar.Configuration.Destroyable) return;
        Destroy(gameObject);
    }
}
