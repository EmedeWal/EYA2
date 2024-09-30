using UnityEngine;

public class GhostStance : StanceBase
{
    //[Header("PASSIVE")]
    //[SerializeField] private float _dashCooldownModifier = 2f;

    //[Header("ULTIMATE")]
    //[SerializeField] private float _timeSlowPercentage = 50f;
    //[SerializeField] private float _movementSpeedBoostPercentage = 20f;
    //[SerializeField] private float _attackDamageBoostPercentage = 100f;

    public override void Enter()
    {
        base.Enter();
        // Effect goes here
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void CastUltimate()
    {
        base.CastUltimate();
        IgnoreCollisions(true);
        //TimeSystem.Instance.SetTimeScale(1 - (_timeSlowPercentage / 100));
        //_DataManager.AttackStruct.AttackModifier = 1 + (_attackDamageBoostPercentage / 100);
        //_DataManager.LocomotionStruct.SpeedModifier = 1 + (_movementSpeedBoostPercentage / 100);
        ////Invoke(nameof(EndUltimate), UltimateDuration);
    }

    public override void DeactivateUltimate()
    {
        base.DeactivateUltimate();
        IgnoreCollisions(false);    
    }

    private void IgnoreCollisions(bool ignore)
    {
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("DamageCollider"), ignore);
    }
}
