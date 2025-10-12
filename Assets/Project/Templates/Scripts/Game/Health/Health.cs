using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using BreakInfinity;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, IStateBarCollection<HealthBar>
{
    public UnityEvent OnDeath;
    public UnityEvent<HealthBar> OnAddBar;
    public bool Dead { get; private set; }
    /// <summary>
    ///     Тип повреждения
    /// </summary>
    public enum DamageType
    {
        [Tooltip("Тип повреждения от пореза")] 
        СUTTING,

        [Tooltip("Тип повреждения от прокалывания")]
        PRICKING,

        [Tooltip("Тип повреждения от удара")] 
        PUNCH,

        [Tooltip("Тип повреждения от взрыва")] 
        EXPLOSION,

        [Tooltip("Тип повреждения от выстрела")]
        SHOOTING,

        [Tooltip("Тип повреждения от перегрева")]
        OVERHEAT,

        [Tooltip("Тип повреждения от обморожения")]
        FROSTBITE,

        [Tooltip("Тип повреждения от электрического разряда")]
        ELECTRIC_SHOCK,

        [Tooltip("Тип повреждения от растворения в кислоте")]
        DISSOLUTION_IN_ACID,

        [Tooltip("Тип повреждения от яда")] 
        POISON,

        [Tooltip("Тип лечения")]
        HEALING,

        [Tooltip("Тип повреждения от чистой магии")]
        MAGIC,
        
        [Tooltip("Тип повреждения от света")]
        LIGHT,        
        
        [Tooltip("Тип повреждения от тьмы")]
        DARK,
        
        [Tooltip("Тип абсолютного повреждения (например, отдельный тип для оружия, которое наносит неизбежный урон)")]
        ABSOLUTE
    }
    private const float FULL_RESIST = 100f;
    
    [field:SerializeField, SerializedDictionary("Тип урона", "Сопротивляемость")]
    public SerializedDictionary<DamageType, float> Resists  { get; private set; } = new ();

    [field:SerializeField] public List<HealthBar> Bars { get; private set; } = new ();
    [SerializeField] private List<HealthBarConfiguration> _configuration;
    private void Start()
    {
        foreach (HealthBar healthBar in _configuration.Select(configuration => new HealthBar(this, configuration)))
        {
            Bars.Add(healthBar);
            healthBar.OnEmpty += OnEmpty;
            OnAddBar?.Invoke(healthBar);
        }
    }

    private void OnDestroy()
    {
        foreach (HealthBar healthBar in Bars)
            healthBar.OnEmpty -= OnEmpty;
    }

    /// <summary>
    /// Получение урона
    /// </summary>
    /// <param name="type">тип урона</param>
    /// <param name="damage">урон</param>
    public void TakeDamage(DamageType type, BigDouble damage)
    {
        if (Resists.TryGetValue(type, out float resist))
            damage *= new BigDouble((FULL_RESIST - resist) / FULL_RESIST);

        for (int i = Bars.Count - 1; i >= 0; i--)
        {
            if (Bars[i].Value > BigDouble.Zero)
            {
                Bars[i].TryChangeValue(type, damage);
                break;
            }
        }
    }

    public void Reset()
    {
        foreach (HealthBar bar in Bars) bar.Reset();
    }
    
    private void OnEmpty(StateBar bar)
    {
        HealthBar emptyHealthBar = (HealthBar)bar;
        
        bool allBarsEmpty = Bars.All(healthBar => healthBar.Value == BigDouble.Zero);
        if (emptyHealthBar.Configuration.Destroyable)
        {
            emptyHealthBar.OnEmpty -= OnEmpty;
            Bars.Remove(emptyHealthBar);
        }

        if (allBarsEmpty || Bars.Count == 0)
        {
            Dead = true;
            OnDeath?.Invoke();
        }
    }

}
[Serializable]
public class Resist
{
    public Health.DamageType DamageType;
    public float Value;
}