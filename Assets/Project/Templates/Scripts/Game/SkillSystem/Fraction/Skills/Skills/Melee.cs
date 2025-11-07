#region

using UnityEngine;

#endregion

[AddComponentMenu("Skill/Melee")]
public class Melee /* : Skill*/ {

    /*/// <summary>
    ///     Промахи не тратят ману и выносливость
    /// </summary>
    [SerializeField] private bool _canMiss;

    public override void Run(Unit initiator, Unit target = null) {
        base.Run(initiator, target);

        // Находим все коллайдеры в радиусе действия умения
        var colliders2D = Physics2D.OverlapCircleAll(initiator.transform.position, Range, LayerMask.GetMask("Person"));

        // Счетчик целей, пораженных умением
        int countCatch = SetEffectsCatch(initiator, colliders2D);

        // Если не была поражена ни одна цель и умение может промахнуться,
        // то персонаж игрока возвращается к использованию умения и анимация удаляется.
        // Иначе, персонаж игрока начинает анимацию умения.
        if (countCatch == 0 && _canMiss)
        {
            initiator.ReturnUseSkill(this);
            initiator.RemoveStateAnimation(NameAnimation);
        }
        else
        {
            if (Consumable)
                initiator.AmountSkill[this]--;
            if (countCatch != 0)
                initiator.ChangeStateAnimation(NameAnimation, 1);
        }
    }

    /// <summary>
    ///     Счетчик целей, пораженных умением
    /// </summary>
    /// <param name="initiator"></param>
    /// <param name="colliders2D"></param>
    /// <returns>Счетчик целей, пораженных умением</returns>
    private int SetEffectsCatch(SerializedDictionarySampleTwo.Person initiator, Collider2D[] colliders2D) {
        int countCatch = 0;

        for (int i = 0; i < colliders2D.Length; i++)
        {
            if (!colliders2D[i].TryGetComponent(out SerializedDictionarySampleTwo.Person target)) continue;

            // Если у цели нет здоровья, переходим к следующей цели
            if (target.Health <= 0) continue;

            // Наносим урон и применяем эффекты умения
            if (OnTrigger(TriggerTarget, initiator, target))
            {
                countCatch++;
                SetEffectsAndBuffs(initiator, target);
            }

            // Если умение может заставить цель двигаться и цель была поражена, персонаж игрока начинает следовать за этой целью на некоторое время
            if (TargetMove && countCatch > 0) _ = initiator.Pursuit(target, ITargetMove(initiator, TimeTargetMove));

            // Если количество пораженных целей достигло максимального значения и это значение не равно 0, то оставшиеся цели не поражаются
            if (countCatch >= MaxCountCatch) break;
        }

        return countCatch;
    }

    public override void Run(SerializedDictionarySampleTwo.Person initiator, Vector3 target) {
        Debug.LogError("Эта способность не может быть направлена на точку");
    }

    private static IEnumerator ITargetMove(SerializedDictionarySampleTwo.Person initiator, float time) {
        if (time <= 0)
            yield break;
        initiator.Distracted = true;

        yield return new WaitForSeconds(time);
        initiator.Distracted = false;
    }*/

}