using System.Linq;
using AYellowpaper.SerializedCollections;
using BreakInfinity;
using UnityEngine;
using static RandomExtensions;

public class Attack : Effect
{
    [field:SerializeField, SerializedDictionary("Тип урона", "Урон")]
    public SerializedDictionary<Health.DamageType, BigDouble> Damages {get; private set;}

    [Min(0)][field: SerializeField] public float CreteChance {get; private set;}
    [Min(0)][field: SerializeField] public BigDouble Crete {get; private set;}
    
    public BigDouble GetSumDamage()
    {
        return Damages.Aggregate(BigDouble.Zero, (current, damage) => current + damage.Value);
    }

    public override void Run(Unit initiator, Unit target)
    {
        BigDouble create = Chance(CreteChance) ? Crete : 0;
        target.Health.TakeDamage(Damages, create, initiator);
    }
}