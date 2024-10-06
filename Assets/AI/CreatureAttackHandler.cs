using System.Collections.Generic;
using UnityEngine;

public class CreatureAttackHandler : AttackHandler
{
    [Header("DATA REFERENCES")]
    [SerializeField] private List<AttackData> _attackDataList = new();

    public float DamageModifier { private get; set; }

    public override void Init(LayerMask targetLayer)
    {
        base.Init(targetLayer);
    }

    public void StartAttack()
    {
        if (_AnimatorManager.GetBool("InAction") || IsAttacking) return;
        int randomIndex = Random.Range(0, _attackDataList.Count);
        HandleAttack(_attackDataList[randomIndex]);
    }

    protected override void HandleDamage(float damage, bool crit)
    {
        base.HandleDamage(damage, crit);
    }

    protected override void AttackEnd()
    {
        Invoke(nameof(ResetIsAttacking), _AttackData.Recovery);
    }

    protected override bool RollCritical()
    {
        return false;
    }

    private void ResetIsAttacking()
    {
        IsAttacking = false;
    }
}
