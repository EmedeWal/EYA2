using System.Collections.Generic;
using UnityEngine;

public class CreatureAttackHandler : AttackHandler
{
    [Header("DATA REFERENCES")]
    [SerializeField] private List<AttackData> _attackDataList = new();

    public float DamageModifier { private get; set; } = 1;

    public override void Init(LayerMask targetLayer)
    {
        base.Init(targetLayer);
    }

    public override void AttackBegin()
    {
        base.AttackBegin();

        _AnimatorManager.Animator.SetBool("IsAttacking", IsAttacking);
    }

    public override void AttackMiddle()
    {
        base.AttackMiddle();
    }

    public override void AttackEnd()
    {
        base.AttackEnd();

        Invoke(nameof(ResetIsAttacking), _AttackData.Recovery);
    }

    public void Attack()
    {
        if (IsAttacking) return;
        int randomIndex = Random.Range(0, _attackDataList.Count);
        HandleAttack(_attackDataList[randomIndex]);
    }

    protected override float HandleCritical(Collider hit, float damage, bool crit)
    {
        return base.HandleCritical(hit, damage * DamageModifier, crit);
    }

    private void ResetIsAttacking()
    {
        IsAttacking = false;
        _AnimatorManager.Animator.SetBool("IsAttacking", IsAttacking);
    }
}