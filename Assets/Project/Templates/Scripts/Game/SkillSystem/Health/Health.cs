using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using BreakInfinity;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, IStateBarCollection<HealthBar> {

    /// <summary>
    ///     Тип повреждения
    /// </summary>
    public enum DamageType {

        [Tooltip("Тип повреждения от пореза")] Сutting,

        [Tooltip("Тип повреждения от прокалывания")]
        Pricking,

        [Tooltip("Тип повреждения от удара")] Punch,

        [Tooltip("Тип повреждения от взрыва")] Explosion,

        [Tooltip("Тип повреждения от выстрела")]
        Shooting,

        [Tooltip("Тип повреждения от перегрева")]
        Overheat,

        [Tooltip("Тип повреждения от обморожения")]
        Frostbite,

        [Tooltip("Тип повреждения от электрического разряда")]
        ElectricShock,

        [Tooltip("Тип повреждения от растворения в кислоте")]
        DissolutionInAcid,

        [Tooltip("Тип повреждения от яда")] Poison,

        [Tooltip("Тип лечения")] Healing,

        [Tooltip("Тип повреждения от чистой магии")]
        Magic,

        [Tooltip("Тип повреждения от света")] Light,

        [Tooltip("Тип повреждения от тьмы")] Dark,

        [Tooltip("Тип абсолютного повреждения (например, отдельный тип для оружия, которое наносит неизбежный урон)")]
        Absolute

    }

    private const float FULL_RESIST = 100f;
    public UnityEvent OnDeath;
    public UnityEvent<HealthBar> OnAddBar;

    [field: SerializeField, SerializedDictionary("Тип урона", "Сопротивляемость")]
    public SerializedDictionary<DamageType, float> Resists { get; private set; } = new();

    [SerializeField] private List<HealthBarConfiguration> _configuration;
    public Action<Dictionary<DamageType, BigDouble>, BigDouble, Unit> OnDamageStart, OnDamageEnd;
    public bool Dead { get; private set; }

    public void Reset() {
        foreach (HealthBar bar in Bars) bar.Reset();
    }

    private void Start() {
        foreach (HealthBar healthBar in _configuration.Select(configuration => new HealthBar(this, configuration)))
        {
            Bars.Add(healthBar);
            healthBar.OnEmpty += OnEmpty;
            OnAddBar?.Invoke(healthBar);
        }
    }

    private void OnDestroy() {
        foreach (HealthBar healthBar in Bars)
            healthBar.OnEmpty -= OnEmpty;
    }

    [field: SerializeField] public List<HealthBar> Bars { get; private set; } = new();

    /// <summary>
    ///     Получение урона
    /// </summary>
    /// <param name="damages">Урон</param>
    /// <param name="create">Крит</param>
    /// <param name="initiator">Инициатор</param>
    public void TakeDamage(Dictionary<DamageType, BigDouble> damages, BigDouble create, Unit initiator) {
        OnDamageStart?.Invoke(damages, create, initiator);

        foreach (var damageElement in damages)
        {
            BigDouble damage = damageElement.Value;
            DamageType type = damageElement.Key;

            if (Resists.TryGetValue(type, out float resist))
                damage *= new BigDouble((FULL_RESIST - resist) / FULL_RESIST);

            for (int i = Bars.Count - 1; i >= 0; i--)
                if (Bars[i].Value > BigDouble.Zero)
                {
                    Bars[i].TryChangeValue(type, damage);

                    break;
                }
        }

        OnDamageEnd?.Invoke(damages, create, initiator);
    }

    private void OnEmpty(StateBar bar) {
        HealthBar emptyHealthBar = (HealthBar)bar;

        bool allBarsEmpty = Bars.All(healthBar => healthBar.Value == 0);

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
public class Resist {

    public Health.DamageType DamageType;
    public float Value;

}