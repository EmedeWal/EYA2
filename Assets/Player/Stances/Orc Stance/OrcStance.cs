using UnityEngine;

public class OrcStance : StanceBase
{
    [Header("PASSIVE")]
    [SerializeField] private GameObject _splashPrefab;

    [Header("ULTIMATE")]
    [SerializeField] private float _damageReductionPercentage;

    public override void Init()
    {
        base.Init();
    }

    public override void Enter()
    {
        base.Enter();
        _AttackHandler.SuccessfulAttack += OrcStance_SuccesfulAttack;
    }

    public override void Exit()
    {
        base.Exit();
        _AttackHandler.SuccessfulAttack -= OrcStance_SuccesfulAttack;
    }

    public override void CastUltimate()
    {
        base.CastUltimate();
        _Health.SetDamageReduction(_damageReductionPercentage / 100);
    }

    public override void DeactivateUltimate()
    {
        base.DeactivateUltimate();
        _Health.SetDamageReduction(0);
    }

    private void OrcStance_SuccesfulAttack(Collider hit, float damage)
    {
        Explosion explosion = Instantiate(_splashPrefab, hit.transform.position, hit.transform.rotation).GetComponent<Explosion>();
        explosion.SetColliderToIgnore(hit);
        explosion.SetDamage(damage / 2);
    }
}
