#region

using UnityEngine;

#endregion

[AddComponentMenu("Skill/Projectile")]
public class Projectile /* : Skill*/ {

    /*[MinMaxSlider(1, 100, "MaxCountProjectile", "Count Projectile"), SerializeField]
    public int MinCountProjectile;

    [HideInInspector] public int MaxCountProjectile;

    [FormerlySerializedAs("timeDanger"), Min(0)]
    public float TimeDanger;

    [FormerlySerializedAs("timeDead"), Min(0)]
    public float TimeDead;

    [FormerlySerializedAs("speed"), Min(0)]
    public float Speed;

    [FormerlySerializedAs("offset")] public float Offset;

    /// <summary>
    ///     разброс
    /// </summary>
    [FormerlySerializedAs("scatter"), Range(0f, 360f)]
    public float Scatter;

    [FormerlySerializedAs("targetPerson")] public bool TargetPerson;

    private ProjectileObject _projectilePrefab;

    private void SpawnProjectile(Unit initiator, Unit target) {
        ProjectileObject projectile = Instantiate(_projectilePrefab, initiator.transform.parent);

        projectile.transform.position = initiator.transform.position + initiator.transform.up * Offset;
        projectile.transform.LookAt2D(target.transform.position);
        projectile.transform.eulerAngles =
            projectile.transform.eulerAngles.Z(projectile.transform.eulerAngles.z + Random.Range(-Scatter, Scatter));
        projectile.Build(initiator, this, target);
    }

    private void SpawnProjectile(Unit initiator, Vector3 target) {
        ProjectileObject projectileObject = Instantiate(_projectilePrefab, initiator.transform.parent);

        projectileObject.transform.position = initiator.transform.position + initiator.transform.up * Offset;
        projectileObject.transform.LookAt2D(target);
        projectileObject.transform.eulerAngles =
            projectileObject.transform.eulerAngles.Z(projectileObject.transform.eulerAngles.z +
                                                     Random.Range(-Scatter, Scatter));
        projectileObject.Build(initiator, this);
    }

    public override void Run(Unit initiator, Unit target = null) {
        base.Run(initiator, target);

        if (!target || !LimitRun(initiator, target.transform.position))
            return;

        if (Consumable)
            initiator.AmountSkill[this]--;

        for (int i = 0; i < Random.Range(MinCountProjectile, MaxCountProjectile); i++)
            SpawnProjectile(initiator, target);
    }

    public override void Run(Unit initiator, Vector3 target) {
        base.Run(initiator, null);

        if (TargetPerson || !PointCanBeTarget)
            return;
        if (!LimitRun(initiator, target))
            return;

        if (Consumable)
            initiator.AmountSkill[this]--;

        for (int i = 0; i < Random.Range(MinCountProjectile, MaxCountProjectile); i++)
            SpawnProjectile(initiator, target);
    }*/

}