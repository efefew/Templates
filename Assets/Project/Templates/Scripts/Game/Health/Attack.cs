using BreakInfinity;
using UnityEngine;
using static RandomExtensions;

public class Attack : MonoBehaviour
{
    [Min(0)][field: SerializeField] public BigDouble Damage {get; private set;}
    [field: SerializeField] public Health.DamageType DamageType {get; private set;}
    [Min(0)][field: SerializeField] public float CreteChance {get; private set;}
    [Min(0)][field: SerializeField] public BigDouble Crete {get; private set;}
    
    /// <summary>
    /// Нанесение урона
    /// </summary>
    /// <param name="enemy">Тот, кому наносят урон</param>
    public void GiveDamage(Health enemy)
    {
        BigDouble damage = Chance(CreteChance) ? Damage * Crete : Damage;
        enemy.TakeDamage(DamageType, damage);
    }
}
