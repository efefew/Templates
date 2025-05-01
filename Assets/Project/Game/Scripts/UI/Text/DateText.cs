using System;
using System.Globalization;
using TMPro;
using UnityEngine;
[RequireComponent(typeof(TMP_Text))]
public class DateText : MonoBehaviour
{
    [SerializeField]
    private string _format = "dd MMMM yyyy", _culture = "en-US";
    [SerializeField]
    private int _shiftDay;
    private void Start()
    {
        TMP_Text tmpText = GetComponent<TMP_Text>();
        DateTime dateTime = DateTime.Now;
        dateTime = dateTime.AddDays(_shiftDay);
        tmpText.text = dateTime.ToString(_format, new CultureInfo(_culture));
    }
}
