using UnityEngine;

public class VampireStance : Stance, IStance
{
    [Header("PASSIVE")]
    [SerializeField] private float _totalBleedDamage = 20f;
    [SerializeField] private float _totalBleedDuration = 5f;

    [Header("UTLIMATE")]
    [SerializeField] private float _lifestealPercentage;

    private PlayerLightAttack _lightAttack;

    private void Start()
    {
        _lightAttack = GetComponent<PlayerLightAttack>();
    }

    public void Enter()
    {
        ManageStanceSwap();
        _lightAttack.SuccesfulLightAttack += VampireStance_SuccesfulLightAttack;
    }

    public void Exit()
    {
        _lightAttack.SuccesfulLightAttack -= VampireStance_SuccesfulLightAttack;
    }

    public void CastUltimate()
    {
        DataManager.SetLifeSteal(_lifestealPercentage / 100);

        Invoke(nameof(EndUltimate), UltimateDuration);

        ActivateUltimate();
    }

    private void EndUltimate()
    {
        DataManager.SetLifeSteal(0);

        DeactivateUltimate();
    }

    private void VampireStance_SuccesfulLightAttack(Collider hit)
    {
        if (!hit.TryGetComponent<EnemyHealth>(out var enemyHealth)) return;

        enemyHealth.Bleed(_totalBleedDamage, _totalBleedDuration);
    }
}
