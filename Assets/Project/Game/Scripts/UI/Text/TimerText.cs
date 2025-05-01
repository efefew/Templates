using System;
using System.Globalization;
using TMPro;
using UnityEngine;
[RequireComponent(typeof(TMP_Text))]
public class TimerText : MonoBehaviour
{
    private TMP_Text _text;
    private DateTime _time;
    private bool _isActive;
    private void Start()
    {
        _text = GetComponent<TMP_Text>();
    }

    public void StartTimer()
    {
        _isActive = true;
        _time = DateTime.Now;
    }
    public void StopTimer()
    {
        _isActive = false;
    }

    private void Update()
    {
        if(!_isActive) return;
        TimeSpan duration = DateTime.Now - _time;
        _text.text = $"{(int)duration.TotalHours:00}:{duration.Minutes:00}:{duration.Seconds:00}";
    }
}
