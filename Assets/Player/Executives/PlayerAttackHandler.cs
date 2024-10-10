using UnityEngine;
using System;

public class PlayerAttackHandler : AttackHandler
{
    [Header("PLAYER STATS")]
    [SerializeField] private PlayerStats _stats;

    [Header("ATTACK DATA")]
    [SerializeField] private AttackData _lightAttackData;
    [SerializeField] private AttackData _heavyAttackData;

    private PlayerInputHandler _inputHandler;

    public bool GuaranteedCrit { private get; set; } = false;

    public event Action<AttackType> AttackStarted;
    public event Action<AttackType> AttackFinished;

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
        OnAttackFinished(_AttackData.AttackType);
        IsAttacking = false;
    }

    protected override bool RollCritical()
    {
        if (GuaranteedCrit || Helpers.GetChanceRoll(_stats.GetCurrentStat(Stat.CriticalChance)))
        {
            GuaranteedCrit = false; return true;
        }
        return false;
    }

    private void PlayerAttackHandler_LightAttackInputPerformed()
    {
        if (_AnimatorManager.GetBool("InAction") || IsAttacking) return;
        OnAttackStarted(_lightAttackData.AttackType);
        HandleAttack(_lightAttackData);
    }

    private void PlayerAttackHandler_HeavyAttackInputPerformed()
    {
        if (_AnimatorManager.GetBool("InAction") || IsAttacking) return;
        OnAttackStarted(_heavyAttackData.AttackType);
        HandleAttack(_heavyAttackData);
    }

    private void OnAttackStarted(AttackType attackType)
    {
        AttackStarted?.Invoke(attackType);
    }

    private void OnAttackFinished(AttackType attackType)
    {
        AttackFinished?.Invoke(attackType);
    }
}