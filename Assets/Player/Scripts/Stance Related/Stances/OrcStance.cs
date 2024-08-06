using UnityEngine;

public class OrcStance : Stance, IStance
{
    [Header("PASSIVE")]
    [SerializeField] private GameObject _splashPrefab;

    [Header("ULTIMATE")]
    [SerializeField] private float _damageReductionPercentage;

    private PlayerHeavyAttack _heavyAttack;
    private Health _health;

    private void Start()
    {
        _health = GetComponent<Health>();
    }

    private void OnDisable()
    {
        PlayerAttack.SuccesfulAttack -= OrcStance_SuccesfulAttack;
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
        PlayerAttack.SuccesfulAttack += OrcStance_SuccesfulAttack;
    }

    public void Exit()
    {
        PlayerAttack.SuccesfulAttack -= OrcStance_SuccesfulAttack;
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
