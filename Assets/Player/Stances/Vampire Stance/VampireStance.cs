using UnityEngine;

public class VampireStance : StanceBase
{
    [Header("PASSIVE")]
    [SerializeField] private float _totalBleedDuration;

    [Header("ULTIMATE")]
    [SerializeField] private float _lifeStealPercentage;

    public override void Enter()
    {
        base.Enter();
        _AttackHandler.SuccessfulAttack += VampireStance_SuccesfulAttack;
    }

    public override void Exit()
    {
        base.Exit();
        _AttackHandler.SuccessfulAttack -= VampireStance_SuccesfulAttack;
    }

    public override void CastUltimate()
    {
        base.CastUltimate();
        _DataManager.StanceStruct.LifeSteal += _lifeStealPercentage / 100;
    }

    public override void DeactivateUltimate()
    {
        base.DeactivateUltimate();
        _DataManager.StanceStruct.LifeSteal -= _lifeStealPercentage / 100;
    }

    private void VampireStance_SuccesfulAttack(Collider hit, float damage)
    {
        if (!hit.TryGetComponent<Bleed>(out var bleed)) return;
        bleed.InflictBleed(damage / 2, _totalBleedDuration);
    }
}
