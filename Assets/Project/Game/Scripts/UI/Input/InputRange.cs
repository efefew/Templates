using System.Globalization;
using TMPro;
using UnityEngine;
[RequireComponent(typeof(TMP_InputField))]
public class InputRange : MonoBehaviour
{
    private TMP_InputField _inputField;
    [SerializeField] private bool _needMin;
    [SerializeField] private float _min;
    [SerializeField] private bool _needMax;
    [SerializeField] private float _max;
    private void Start()
    {
        _inputField = GetComponent<TMP_InputField>();
        _inputField.onValueChanged.AddListener(OnInput);
    }

    private void OnInput(string input)
    {
        if (_needMin && _needMax && _min > _max)
        {
            (_min, _max) = (_max, _min);
        }
        
        if(!float.TryParse(input, out float value)) return;
        
        if (_needMin)
        {
            value = Mathf.Max(value, _min);
        }
        if (_needMax)
        {
            value = Mathf.Min(value, _max);
        }
        _inputField.text = value.ToString(CultureInfo.InvariantCulture);
    }
}