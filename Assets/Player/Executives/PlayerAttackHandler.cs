using UnityEngine;

public class PlayerAttackHandler : AttackHandler
{
    [Header("PLAYER STATS")]
    [SerializeField] private PlayerStats _stats;

    [Header("ATTACK DATA")]
    [SerializeField] private AttackData _lightAttackData;
    [SerializeField] private AttackData _heavyAttackData;

    [Header("CRIT EFFECTS")]
    [SerializeField] private VFX _critVFX;

    private PlayerInputHandler _inputHandler;

    public bool GuaranteedCrit { private get; set; } = false;

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

    public override void AttackBegin()
    {
        base.AttackBegin();
    }

    public override void AttackMiddle()
    {
        base.AttackMiddle();
    }

    public override void AttackEnd()
    {
        base.AttackEnd();
    }

    protected override float HandleCritical(Collider hit, float damage, bool crit)
    {
        if (crit)
        {
            if (hit.TryGetComponent(out LockTarget lockTarget))
            {
                Transform center = lockTarget.Center;
                VFX critVFX = _VFXManager.AddMovingVFX(_critVFX, center, 1f);

                AudioSource source = critVFX.GetComponent<AudioSource>();
                _AudioSystem.PlayAudio(source, source.clip, source.volume, 0.05f);
            }

            return damage * _stats.GetCurrentStat(Stat.CriticalMultiplier);
        }
        else
        {
            return damage;
        }
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
        HandleAttack(_lightAttackData);
    }

    private void PlayerAttackHandler_HeavyAttackInputPerformed()
    {
        HandleAttack(_heavyAttackData);
    }
}