using UnityEngine;

public class PlayerAttackHandler : AttackHandler
{
    [Header("PLAYER STATS")]
    [SerializeField] private PlayerStats _stats;

    [Header("ATTACK DATA")]
    [SerializeField] private AttackData _lightAttackData;
    [SerializeField] private AttackData _heavyAttackData;

    private PlayerInputHandler _inputHandler;

    public override void Init(LayerMask targetLayer)
    {
        base.Init(targetLayer);

        _inputHandler = GetComponent<PlayerInputHandler>();

        _inputHandler.LightAttackInputPerformed += PlayerAttackHandler_LightAttackInputPerformed;
        _inputHandler.HeavyAttackInputPerformed += PlayerAttackHandler_HeavyAttackInputPerformed;
    }

    public void Cleanup()
    {
        _inputHandler.LightAttackInputPerformed -= PlayerAttackHandler_LightAttackInputPerformed;
        _inputHandler.HeavyAttackInputPerformed -= PlayerAttackHandler_HeavyAttackInputPerformed;
    }

    protected override void HandleDamage(float damage, bool crit)
    {
        if (crit)
        {
            damage *= _stats.GetCurrentStat(Stat.CriticalMultiplier);
        }

        base.HandleDamage(damage, crit);
    }

    protected override void AttackEnd()
    {
        IsAttacking = false;
    }

    protected override bool RollCritical()
    {
        return Helpers.GetChanceRoll(_stats.GetCurrentStat(Stat.CriticalChance));
    }

    private void PlayerAttackHandler_LightAttackInputPerformed()
    {
        if (_AnimatorManager.GetBool("InAction") || IsAttacking) return;
        HandleAttack(_lightAttackData);
    }

    private void PlayerAttackHandler_HeavyAttackInputPerformed()
    {
        if (_AnimatorManager.GetBool("InAction") || IsAttacking) return;
        HandleAttack(_heavyAttackData);
    }
}
