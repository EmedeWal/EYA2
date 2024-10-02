using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class CreatureAttackHandler : AttackHandler
{
    [Header("DATA REFERENCES")]
    [SerializeField] private List<AttackData> _attackDataList = new();

    public CreatureData CreatureData { private get; set; }

    public override void Init(LayerMask targetLayer)
    {
        base.Init(targetLayer);
    }

    public void StartAttack()
    {
        int randomIndex = Random.Range(0, _attackDataList.Count);
        HandleAttack(_attackDataList[randomIndex]);
    }

    protected override void HandleDamage(float damage, bool crit)
    {
        base.HandleDamage(damage, crit);
    }

    protected override void AttackEnd()
    {
        StartCoroutine(AttackEndCoroutine());
        IEnumerator AttackEndCoroutine()
        {
            yield return new WaitForSeconds(CreatureData.Recovery);

            IsAttacking = false;
        }
    }

    protected override bool RollCritical()
    {
        return false;
    }
}
