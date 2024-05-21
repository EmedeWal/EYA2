using UnityEngine;

public class GhostStance : Stance, IStance
{
    [Header("GHOST STANCE")]
    [SerializeField] private float _dashCooldownModifier = 2;
    [SerializeField] private GameObject _explosionPrefab;

    public void Enter()
    {
        ManageStanceSwap();
        DataManager.SetDashModifier(_dashCooldownModifier);
    }

    public void Exit()
    {
        DataManager.SetDashModifier(1);
    }

    public void CastUltimate()
    {
        PlayerDash.DashEnd += GhostStance_DashEnd;

        IgnoreCollisions(true);

        Invoke(nameof(EndUltimate), UltimateDuration);

        ActivateUltimate();
    }

    private void EndUltimate()
    {
        PlayerDash.DashEnd -= GhostStance_DashEnd;

        IgnoreCollisions(false);

        DeactivateUltimate();
    }

    private void GhostStance_DashEnd()
    {
        DashEffect();
    }

    private void DashEffect()
    {
        Instantiate(_explosionPrefab, transform.position, transform.rotation);
    }

    private void IgnoreCollisions(bool ignore)
    {
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), ignore);
    }
}
