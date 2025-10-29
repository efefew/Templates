using System;
using BreakInfinity;
using static Health;
[Serializable]
public class HealthBar : StateBar
{
    private Health _health;
    private DamageType _damageType = DamageType.Absolute;
    [field:NonSerialized] public new HealthBarConfiguration Configuration { get; protected set; }
    public HealthBar(Health health, HealthBarConfiguration configuration) : base(configuration)
    {
        Configuration = configuration;
        _health = health;
    }
    public void TryChangeValue(DamageType damageType, BigDouble deltaValue)
    {
        _damageType = damageType;
        TryChangeValue(deltaValue);
    }

    public override void TryChangeValue(BigDouble deltaValue)
    {
        foreach (Resist resist in Configuration.Resists)
        {
            if (resist.DamageType == _damageType)
            {
                float resistValue = ((100 - resist.Value) / 100);
                deltaValue *= new BigDouble(resistValue);
            }
        }

        deltaValue = _damageType == DamageType.Healing ? -deltaValue : deltaValue;
        base.TryChangeValue(deltaValue);
    }
}