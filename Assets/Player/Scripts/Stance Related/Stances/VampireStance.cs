using UnityEngine;

public class VampireStance : Stance, IStance
{
    [Header("PASSIVE")]
    [SerializeField] private float _totalBleedDuration = 5f;

    [Header("UTLIMATE")]
    [SerializeField] private float _lifestealPercentage;

    private void OnDisable()
    {
        PlayerAttack.SuccessfulAttack -= VampireStance_SuccesfulAttack;
    }

    public void Enter()
    {
        ManageStanceSwap();
        PlayerAttack.SuccessfulAttack += VampireStance_SuccesfulAttack;
    }

    public void Exit()
    {
        PlayerAttack.SuccessfulAttack -= VampireStance_SuccesfulAttack;
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
