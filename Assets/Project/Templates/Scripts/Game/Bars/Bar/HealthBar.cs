using System.Linq;
using UnityEngine;
using static Health;

public class HealthBar : StateBar
{
    private Health _health;
    private DamageType _damageType = DamageType.ABSOLUTE;
    public new HealthBarConfiguration Configuration { get; protected set; }
    public HealthBar(Health health, HealthBarConfiguration configuration) : base(configuration)
    {
        Configuration = configuration;
        _health = health;
    }
    public void TryChangeValue(DamageType damageType, float deltaValue)
    {
        _damageType = damageType;
        TryChangeValue(deltaValue);
    }

    public override void TryChangeValue(float deltaValue)
    {
        deltaValue = Configuration.Resists.Where(resist => resist.DamageType == _damageType).Aggregate(deltaValue, (current, resist) => current * ((100 - resist.Value) / 100));
        deltaValue = _damageType == DamageType.HEALING ? -deltaValue : deltaValue;
        base.TryChangeValue(deltaValue);
    }
}
