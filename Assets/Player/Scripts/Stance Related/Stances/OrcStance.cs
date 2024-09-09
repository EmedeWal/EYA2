using UnityEngine;

public class OrcStance : Stance, IStance
{
    private PlayerAttackHandler _attackHandler;
    private Health _health;

    [Header("PASSIVE")]
    [SerializeField] private GameObject _splashPrefab;

    [Header("ULTIMATE")]
    [SerializeField] private float _damageReductionPercentage;

    protected override void Awake()
    {
        base.Awake();
        _attackHandler = GetComponent<PlayerAttackHandler>();
        _health = GetComponent<Health>();
    }

    private void OrcStance_SuccesfulAttack(Collider hit, float damage)
    {
        Explosion explosion = Instantiate(_splashPrefab, hit.transform.position, hit.transform.rotation).GetComponent<Explosion>();
        explosion.SetColliderToIgnore(hit);
        explosion.SetDamage(damage / 2);
    }

    public void Enter()
    {
        ManageStanceSwap();
        _attackHandler.SuccessfulAttack += OrcStance_SuccesfulAttack;
    }

    public void Exit()
    {
        _attackHandler.SuccessfulAttack -= OrcStance_SuccesfulAttack;
    }

    public void CastUltimate()
    {
        _health.SetDamageReduction(_damageReductionPercentage / 100);

        Invoke(nameof(EndUltimate), UltimateDuration);

        ActivateUltimate();
    }

    private void EndUltimate()
    {
        _health.SetDamageReduction(0);

        DeactivateUltimate();
    }
}
