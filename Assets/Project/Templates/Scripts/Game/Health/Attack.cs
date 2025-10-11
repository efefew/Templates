using UnityEngine;
using static RandomExtensions;

public class Attack : MonoBehaviour
{
    [Min(0)][field: SerializeField] public float Damage {get; private set;}
    [field: SerializeField] public Health.DamageType DamageType {get; private set;}
    [Min(0)][field: SerializeField] public float CreteChance {get; private set;}
    [Min(0)][field: SerializeField] public float Crete {get; private set;}
    
    /// <summary>
    /// Нанесение урона
    /// </summary>
    /// <param name="enemy">Тот, кому наносят урон</param>
    public void GiveDamage(Health enemy)
    {
        float damage = Chance(CreteChance) ? Damage * Crete : Damage;
        enemy.TakeDamage(DamageType, damage);
    }
}
