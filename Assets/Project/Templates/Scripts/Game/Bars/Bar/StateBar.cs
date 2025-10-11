using System;
using UnityEngine;

public abstract class StateBar
{
    public float Value { get; protected set; }
    public float MaxValue { get; protected set; }
    public StateBarConfiguration Configuration { get; protected set; }
    public event Action<StateBar> OnEmpty, OnChanged;
    protected StateBar(StateBarConfiguration configuration)
    {
        Configuration = configuration;
        Reset();
    }
    public void Reset()
    {
        MaxValue = Configuration.MaxValue;
        Value = MaxValue;
    }
    public void ChangeMax(float maxValue)
    {
        if (maxValue <= 0) return;
        MaxValue = maxValue;
        OnChanged?.Invoke(this);
    }

    public virtual void TryChangeValue(float deltaValue)
    {
        Value -= deltaValue;
        Value = Mathf.Clamp(Value, 0, MaxValue);
        OnChanged?.Invoke(this);
        if(Value == 0) OnEmpty?.Invoke(this);
    }
}
