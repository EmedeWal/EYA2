using UnityEngine;

public class VampireStance : Stance, IStance
{
    private PlayerAttackHandler _attackHandler;

    [Header("PASSIVE")]
    [SerializeField] private float _totalBleedDuration = 5f;

    [Header("UTLIMATE")]
    [SerializeField] private float _lifestealPercentage;

    protected override void Awake()
    {
        base.Awake();
        _attackHandler = GetComponent<PlayerAttackHandler>();
    }

    public void Enter()
    {
        ManageStanceSwap();
        _attackHandler.SuccessfulAttack += VampireStance_SuccesfulAttack;
    }

    public void Exit()
    {
        _attackHandler.SuccessfulAttack -= VampireStance_SuccesfulAttack;
    }

    public void CastUltimate()
    {
        _DataManager.SetLifeSteal(_lifestealPercentage / 100);
        Invoke(nameof(EndUltimate), UltimateDuration);
        ActivateUltimate();
    }

    private void VampireStance_SuccesfulAttack(Collider hit, float damage)
    {
        if (!hit.TryGetComponent<Bleed>(out var bleed)) return;
        bleed.InflictBleed(damage / 2, _totalBleedDuration);
    }

    private void EndUltimate()
    {
        _DataManager.SetLifeSteal(0);
        DeactivateUltimate();
    }
}
