#region

using UnityEngine;

#endregion

[AddComponentMenu("Skill/Aura")]
public class Aura /* : Skill*/ {

    /*
    [MinMaxSlider(1, 100, "maxCountAura", "Count Aura"), SerializeField]
    public int minCountAura;

    [HideInInspector] public int maxCountAura;


    [Min(0.001f)] public float gap;

    public uint frequency;

    [Min(0)] public float radius;

    [Min(0)] public float scatter;
    public AuraObject aura;


    public override void Run(Unit initiator, Unit target = null) {
        base.Run(initiator, target);

        if (!LimitRun(initiator, target.transform.position) || target == null)
            return;

        if (Consumable)
            initiator.AmountSkill[this]--;

        for (int i = 0; i < Random.Range(minCountAura, maxCountAura); i++)
        {
            AuraObject aura = Instantiate(this.aura,
                new Vector2(transform.position.x + Random.Range(0f, scatter),
                    transform.position.y + Random.Range(0f, scatter)), Quaternion.Euler(0, 0, Random.Range(0f, 360f)),
                initiator.transform.parent);
            aura.Build(initiator, this, target);
        }
    }
    */

}