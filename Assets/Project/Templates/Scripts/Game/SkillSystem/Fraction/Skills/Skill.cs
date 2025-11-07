using System;
using UnityEngine;

public abstract class Skill : MonoBehaviour {

    public enum SkillType {

        Attack,
        Defend,
        Move,
        Heal,
        Buff,
        Debuff

    }

    public enum TriggerType {

        Enemy,
        Self,
        Friend,
        SelfAndFriend,
        EnemyAndFriend,
        SelfAndEnemy,
        All

    }

    [field: SerializeField] public Unit Initiator { get; private set; }
    [field: SerializeField, Min(0)] public float Range;

    [field: SerializeField, Min(0), Tooltip("время перезарядки")]
    public float TimeCooldown { get; private set; }

    [field: SerializeField, Min(0), Tooltip("время заряда навыка")]
    public float TimeCast { get; private set; }

    [field: SerializeField, Min(0), Tooltip("Расходник")]
    public bool Consumable { get; private set; }

    [field: SerializeField, Min(0), Tooltip("Макс. количество (если расходник)")]
    public int MaxAmount { get; private set; }

    [field: SerializeField, Min(0), Tooltip("Количество (если расходник)")]
    public int Amount { get; private set; }

    [field: SerializeField, Min(0)] public float MaxAmountSkill { get; private set; }
    [field: SerializeField] public SkillType Type { get; private set; }

    [field: SerializeField, Tooltip("На кого необходимо воздействовать")]
    public TriggerType TriggerTarget { get; private set; }

    [field: SerializeField, Tooltip("На кого воздействует")]
    public TriggerType TriggerDanger { get; private set; }

    [field: SerializeField] public Buff[] Buffs { get; private set; }
    [field: SerializeField] public Effect[] Effects { get; private set; }

    public static bool OnTrigger(TriggerType trigger, Unit initiator, Unit target) {
        return trigger switch
        {
            TriggerType.Enemy => IsEnemy(initiator, target),
            TriggerType.Self => IsMe(initiator, target),
            TriggerType.Friend => IsFriend(initiator, target),
            TriggerType.SelfAndFriend => !IsEnemy(initiator, target),
            TriggerType.EnemyAndFriend => !IsMe(initiator, target),
            TriggerType.SelfAndEnemy => !IsFriend(initiator, target),
            TriggerType.All => true,
            _ => false
        };
    }

    public static bool ContainsTrigger(TriggerType trigger, TriggerType target) {
        if (target is TriggerType.All) return true;

        return trigger switch
        {
            TriggerType.Enemy => target is TriggerType.Enemy or TriggerType.EnemyAndFriend or TriggerType.SelfAndEnemy,
            TriggerType.Self => target is TriggerType.Self or TriggerType.SelfAndFriend or TriggerType.SelfAndEnemy,
            TriggerType.Friend => target is TriggerType.Friend or TriggerType.EnemyAndFriend
                or TriggerType.SelfAndFriend,
            TriggerType.SelfAndFriend => target is not TriggerType.Enemy,
            TriggerType.EnemyAndFriend => target is not TriggerType.Self,
            TriggerType.SelfAndEnemy => target is not TriggerType.Friend,
            TriggerType.All => true,
            _ => false
        };
    }

    private static bool IsEnemy(Unit initiator, Unit target) {
        return target.Fraction != initiator.Fraction;
    }

    private static bool IsFriend(Unit initiator, Unit target) {
        return target.Fraction == initiator.Fraction && target != initiator;
    }

    private static bool IsMe(Unit initiator, Unit target) {
        return initiator == target;
    }

    public static (bool, bool, bool) GetTrigger(TriggerType trigger) {
        bool enemy = false;
        bool self = false;
        bool friend = false;

        switch (trigger)
        {
            case TriggerType.Enemy:
                enemy = true;

                break;

            case TriggerType.Self:
                self = true;

                break;

            case TriggerType.Friend:
                friend = true;

                break;

            case TriggerType.SelfAndFriend:
                self = true;
                friend = true;

                break;

            case TriggerType.EnemyAndFriend:
                enemy = true;
                friend = true;

                break;

            case TriggerType.SelfAndEnemy:
                self = true;
                enemy = true;

                break;

            case TriggerType.All:
                enemy = true;
                self = true;
                friend = true;

                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(trigger), trigger, null);
        }

        return (enemy, self, friend);
    }

    protected void SetEffectsAndBuffs(Unit initiator, Unit target) {
        foreach (Effect effect in Effects) effect.Run(initiator, target);

        foreach (Buff buff in Buffs) buff.Run(initiator, target);
    }

    /// <summary>
    ///     Реализация навыка
    /// </summary>
    /// <param name="target">Цель навыка</param>
    public abstract void Run(Unit target = null);

    /// <summary>
    ///     Реализация навыка
    /// </summary>
    /// <param name="target">цель навыка</param>
    public abstract void Run(Vector3 target);

    /// <summary>
    ///     Проверяет возможность реализации навыка
    /// </summary>
    /// <param name="target">цель навыка</param>
    public bool LimitRun(Vector3 target) {
        // Проверяем, может ли персонаж использовать это умение
        if (!Consumable || !(Amount - 1 < 0))
            return LimitRangeRun(target);

        return false;
    }

    /// <summary>
    ///     Проверяем, может ли персонаж дотянуться до врага этим умением
    /// </summary>
    /// <param name="target">врага</param>
    /// <returns></returns>
    public bool LimitRangeRun(Vector3 target) {
        float distance = Vector2.Distance(Initiator.transform.position, target);

        return !(distance > Range) || Range == 0;
    }

}