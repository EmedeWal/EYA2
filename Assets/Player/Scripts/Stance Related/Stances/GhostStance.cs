using UnityEngine;

public class GhostStance : Stance, IStance
{
    [Header("PASSIVE")]
    [SerializeField] private float _dashCooldownModifier = 2f;

    [Header("ULTIMATE")]
    [SerializeField] private float _timeSlowPercentage = 50f;
    [SerializeField] private float _movementSpeedBoostPercentage = 20f;
    [SerializeField] private float _attackDamageBoostPercentage = 100f;

    public void Enter()
    {
        ManageStanceSwap();
        _DataManager.SetDashModifier(_dashCooldownModifier);
    }

    public void Exit()
    {
        _DataManager.SetDashModifier(1);
    }

    public void CastUltimate()
    {
        TimeManager.Instance.SetTimeScale(1 - (_timeSlowPercentage / 100));
        _DataManager.SetAttackModifier(1 + (_attackDamageBoostPercentage / 100));
        _DataManager.SetMovementModifier(1 + (_movementSpeedBoostPercentage / 100));
        Invoke(nameof(EndUltimate), UltimateDuration);
        IgnoreCollisions(true);
        ActivateUltimate();
    }

    private void EndUltimate()
    {
        TimeManager.Instance.ResetTimeScaleToDefault();
        _DataManager.SetMovementModifier(1);
        _DataManager.SetAttackModifier(1);
        IgnoreCollisions(false);
        DeactivateUltimate();
    }

    private void IgnoreCollisions(bool ignore)
    {
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), ignore);
    }
}
