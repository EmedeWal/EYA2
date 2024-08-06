using UnityEngine;

public class GhostStance : Stance, IStance
{
    [Header("GHOST STANCE")]
    [SerializeField] private float _dashCooldownModifier = 2;
    [SerializeField] private GameObject _explosionPrefab;
    private PlayerDash _playerDash;

    protected override void Awake()
    {
        base.Awake();
        _playerDash = GetComponent<PlayerDash>();
    }

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
        Invoke(nameof(EndUltimate), UltimateDuration);
        _playerDash.DashEnd += GhostStance_DashEnd;
        IgnoreCollisions(true);
        ActivateUltimate();
    }

    private void EndUltimate()
    {
        _playerDash.DashEnd -= GhostStance_DashEnd;
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
