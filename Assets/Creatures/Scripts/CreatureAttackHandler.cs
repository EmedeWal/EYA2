using System.Collections.Generic;
using UnityEngine;

public class CreatureAttackHandler : BaseAttackHandler
{
    [Header("DATA REFERENCES")]
    public List<BaseAttackData> AttackDataList = new();

    public BaseAttackData AttackData { get; private set; } = null;
    public float DamageModifier { private get; set; } = 1;

    public void SelectAttack(BaseAttackData attackData)
    {
        AttackData = attackData;
    }

    public void SelectRandomAttack(List<BaseAttackData> attackDataList = null)
    {
        if (attackDataList == null || attackDataList.Count == 0)
        {
            attackDataList = AttackDataList;
        }

        int randomIndex = Random.Range(0, attackDataList.Count);
        AttackData = attackDataList[randomIndex];
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