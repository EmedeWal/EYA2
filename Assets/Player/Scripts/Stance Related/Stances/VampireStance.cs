using UnityEngine;

public class VampireStance : Stance, IStance
{
    [Header("PASSIVE")]
    [SerializeField] private float _totalBleedDuration = 5f;

    [Header("UTLIMATE")]
    [SerializeField] private float _lifestealPercentage;

    private void OnDisable()
    {
        PlayerAttack.SuccesfulAttack -= VampireStance_SuccesfulAttack;
    }

    private void VampireStance_SuccesfulAttack(Collider hit, float damage)
    {
        if (!hit.TryGetComponent<EnemyHealth>(out var enemyHealth)) return;

        enemyHealth.Bleed(damage / 2, _totalBleedDuration);
    }

    public void Enter()
    {
        ManageStanceSwap();
        PlayerAttack.SuccesfulAttack += VampireStance_SuccesfulAttack;
    }

    public void Exit()
    {
        PlayerAttack.SuccesfulAttack -= VampireStance_SuccesfulAttack;
    }

    public void CastUltimate()
    {
        _DataManager.SetLifeSteal(_lifestealPercentage / 100);

        Invoke(nameof(EndUltimate), UltimateDuration);

        ActivateUltimate();
    }

    private void EndUltimate()
    {
        _DataManager.SetLifeSteal(0);

        DeactivateUltimate();
    }
}
