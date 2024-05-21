using UnityEngine;

public class OrcStance : Stance, IStance
{
    [Header("PASSIVE")]
    [SerializeField] private GameObject _splashPrefab;

    [Header("ULTIMATE")]
    [SerializeField] private float _damageReductionPercentage;

    private PlayerHeavyAttack _heavyAttack;
    private PlayerHealth _health;

    private void Start()
    {
        _heavyAttack = GetComponent<PlayerHeavyAttack>();
        _health = GetComponent<PlayerHealth>();
    }

    public void Enter()
    {
        ManageStanceSwap();
        _heavyAttack.SuccesfulHeavyAttack += OrcStance_SuccesfulHeavyAttack;
    }

    public void Exit()
    {
        _heavyAttack.SuccesfulHeavyAttack -= OrcStance_SuccesfulHeavyAttack;
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

    private void OrcStance_SuccesfulHeavyAttack(Collider hit)
    {
        GameObject splashEffect = Instantiate(_splashPrefab, hit.transform.position, hit.transform.rotation);
        splashEffect.GetComponent<Explosion>().SetColliderToIgnore(hit);
    }
}
