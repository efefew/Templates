using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class LabelTimer : MonoBehaviour
{
    private const long TICKS_PER_SECOND = 10000000;
    [SerializeField] private TMP_Text _time;
    [SerializeField][Min(1)] private long _startSeconds = 90;
    [SerializeField] private BaseUI _gameUI;
    [SerializeField] private UnityEvent  _endTimer;
    
    private DateTime _startTime;
    private long _ticksLeft, _oldTicks;
    private bool _isAddTicks;

    public long TicksLeft
    {
        get => _ticksLeft;
        set
        {
            _time.text = new TimeSpan(value).ToString(@"mm\:ss");
            _ticksLeft = value;
        }
    }
    public void Restart()
    {
        TicksLeft = _startSeconds * TICKS_PER_SECOND;
        _startTime = DateTime.Now;
        _oldTicks = 0;
    }
    private void Update()
    {
        UpdateTimer();
    }
    private void UpdateTimer()
    {
        if (!_gameUI.IsShow)
        {
            if (_isAddTicks)
            {
                _isAddTicks = false;
                _oldTicks = (DateTime.Now - _startTime).Ticks + _oldTicks;
            }
            _startTime = DateTime.Now;
            return;
        }
        _isAddTicks = true;
        long ticks = (DateTime.Now - _startTime).Ticks + _oldTicks;
        if (_startSeconds * TICKS_PER_SECOND <= ticks)
        {
            _gameUI.Hide();
            Restart();
            _endTimer?.Invoke();
            return;
        }
        TicksLeft = _startSeconds * TICKS_PER_SECOND - ticks;
    }
}
