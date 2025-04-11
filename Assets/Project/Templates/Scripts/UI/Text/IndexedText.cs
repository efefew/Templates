using TMPro;

using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class IndexedText : MonoBehaviour
{
    private TMP_Text _label;

    [SerializeField]
    private string[] _values;

    [SerializeField]
    [Multiline]
    private string text;

    #region Unity Methods

    private void Awake()
    {
        _label = GetComponent<TMP_Text>();
        UpdateLabel();
    }

    #endregion Unity Methods

    public void SetValue(int index, string value)
    {
        if (index < 0 || index >= _values.Length)
        {
            return;
        }

        _values[index] = value;
    }

    public void SetValues(params string[] values)
    {
        for (int id = 0; id < values.Length; id++)
        {
            if (id >= _values.Length)
            {
                return;
            }

            _values[id] = values[id];
        }
    }

    public void UpdateLabel()
    {
        for (int id = 0; id < _values.Length; id++)
        {
            _label.text = text.Replace($"{{{id}}}", _values[id]);
        }
    }
}