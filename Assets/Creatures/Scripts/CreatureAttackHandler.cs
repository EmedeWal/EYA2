using System.Collections.Generic;
using UnityEngine;

public class CreatureAttackHandler : AttackHandler
{
    [Header("DATA REFERENCES")]
    [SerializeField] private List<AttackData> _attackDataList = new();
    public AttackData AttackData { get; private set; } = null;
    public float DamageModifier { private get; set; } = 1;

    public override void Init(LayerMask targetLayer)
    {
        base.Init(targetLayer);
    }

    public override void AttackEnd()
    {
        base.AttackEnd();
    }

    public void ChooseAttack(Transform target, float angle)
    {
        Vector3 directionToTarget = (target.position - _Transform.position).normalized;
        float angleToTarget = Vector3.Angle(_Transform.forward, directionToTarget);

        List<AttackData> viableAttacks = new();

        if (angleToTarget <= angle)
        {
            viableAttacks.AddRange(_attackDataList.FindAll(a => a.AttackMode == AttackMode.Lunging || a.AttackMode == AttackMode.Tracking));
        }
        else
        {
            viableAttacks.AddRange(_attackDataList.FindAll(a => a.AttackMode == AttackMode.Tracking));
        }

        int randomIndex = Random.Range(0, viableAttacks.Count);
        AttackData = viableAttacks[randomIndex];
    }

    public void Attack()
    {
        HandleAttack(AttackData);
    }

    protected override float HandleCritical(Collider hit, float damage, bool crit)
    {
        return base.HandleCritical(hit, damage * DamageModifier, crit);
    }
}