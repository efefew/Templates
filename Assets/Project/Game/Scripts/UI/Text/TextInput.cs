using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
[RequireComponent(typeof(TMP_Text))]
public class TextInput : MonoBehaviour
{
    [SerializeField] private TMP_InputField[] _inputs;
    [SerializeField] private TMP_Text[] _inputText;
    [SerializeField] private string[] _input;
    [SerializeField] private string _indexedText = "{0} {1}";
    private void OnEnable()
    {
        TMP_Text text = GetComponent<TMP_Text>();
        List<string> textArray = _inputs.Select(input => input.text).ToList();
        textArray.AddRange(_inputText.Select(input => input.text).ToList());
        textArray.AddRange(_input);
        text.text =  string.Format(_indexedText, textArray.ToArray());
    }
}
