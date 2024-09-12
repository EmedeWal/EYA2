using UnityEngine;

public class GhostStance : Stance, IStance
{
//    [Header("PASSIVE")]
//    [SerializeField] private float _dashCooldownModifier = 2f;

    [Header("ULTIMATE")]
    [SerializeField] private float _timeSlowPercentage = 50f;
    [SerializeField] private float _movementSpeedBoostPercentage = 20f;
    [SerializeField] private float _attackDamageBoostPercentage = 100f;

    public void Enter()
    {
        ManageStanceSwap();
        // Effect goes here
    }

    public void Exit()
    {
        // Effect goes here
    }

    public void CastUltimate()
    {
        TimeManager.Instance.SetTimeScale(1 - (_timeSlowPercentage / 100));
        _DataManager.AttackData.AttackModifier = 1 + (_attackDamageBoostPercentage / 100);
        _DataManager.LocomotionData.SpeedModifier = 1 + (_movementSpeedBoostPercentage / 100);
        Invoke(nameof(EndUltimate), UltimateDuration);
        IgnoreCollisions(true);
        ActivateUltimate();
    }

    private void EndUltimate()
    {
        TimeManager.Instance.ResetTimeScale();
        _DataManager.LocomotionData.SpeedModifier = 1;
        _DataManager.AttackData.AttackModifier = 1;
        IgnoreCollisions(false);
        DeactivateUltimate();
    }

    private void IgnoreCollisions(bool ignore)
    {
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), ignore);
    }
}
