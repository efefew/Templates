using System;
using BreakInfinity;
using UnityEngine;
[Serializable]
public abstract class StateBar
{
    [field:SerializeField] public BigDouble Value { get; protected set; }
    [field:SerializeField] public BigDouble MaxValue { get; protected set; }
    [field:NonSerialized] public StateBarConfiguration Configuration { get; protected set; }
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
    public void ChangeMax(BigDouble maxValue)
    {
        if (maxValue <= BigDouble.Zero) return;
        MaxValue = maxValue;
        OnChanged?.Invoke(this);
    }

    public virtual void TryChangeValue(BigDouble deltaValue)
    {
        Value -= deltaValue;
        Value = Value.Clamp(deltaValue, BigDouble.Zero, MaxValue);
        Debug.Log(Value + " " + deltaValue);
        OnChanged?.Invoke(this);
        if(Value == BigDouble.Zero) OnEmpty?.Invoke(this);
    }
}