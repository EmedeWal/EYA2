using UnityEngine;

public class HybridEnemyAI : EnemyAI
{
    [Header("ATTACK AND FIRE POINTS")]
    [SerializeField] protected Transform AttackPoint;
    [SerializeField] protected Transform FirePoint;

    protected EnemyMeleeData MeleeData;
    protected EnemyRangedData RangedData;
    private bool _canAttack = true;
    private bool _canFire = true;

    protected virtual void Update()
    {
        switch (EnemyState)
        {
            case EnemyState.Chasing:
                Chase();
                break;

            case EnemyState.Charging:
                RotateTowardsPlayer();
                break;

            case EnemyState.Attacking:
                ChargeAttack();
                break;

            case EnemyState.Firing:
                ChargeFire();
                break;
        }
    }

    protected virtual void Chase()
    {
        if (InRange(MeleeData.AttackRange))
        {
            SetAttacking();
        }
        else if (InRange(RangedData.FireRange))
        {
            SetFiring();
        }
        else
        {
            MoveTowardsPlayer();
        }
    }


    protected void ChargeAttack()
    {
        if (!_canAttack) return;

        _canAttack = false;

        SetCharging();
        CancelMovement();
        PlayAnimation(MeleeData.AnimationParameter);
        Invoke(nameof(AttackStart), MeleeData.ChargeDuration);
    }

    private void AttackStart()
    {
        SetAttacking();
        DealDamage(CastBox());
        PlayAudio(MeleeData.AttackClip);
        Invoke(nameof(AttackRecovery), MeleeData.AttackDuration);
        Invoke(nameof(AttackReset), MeleeData.AttackCooldown);
    }

    private void AttackReset()
    {
        _canAttack = true;
    }

    protected virtual void AttackRecovery()
    {
        SetRetreating();
    }

    protected void DealDamage(Collider[] hits)
    {
        PlayAudio(MeleeData.AttackClip);

        foreach (Collider hit in hits)
        {
            if (hit.TryGetComponent<Health>(out var health))
            {
                health.TakeDamage(gameObject, MeleeData.AttackDamage);
            }
        }
    }

    private Collider[] CastBox()
    {
        return Physics.OverlapBox(AttackPoint.position, MeleeData.AttackSize, AttackPoint.rotation, PlayerLayer);
    }

    protected void ChargeFire()
    {
        if (!_canFire) return;

        _canFire = false;

        SetCharging();
        CancelMovement();
        PlayAudio(RangedData.FireClip);
        PlayAnimation(RangedData.AnimationParameter);
        Invoke(nameof(FireStart), RangedData.ChargeDuration);
    }

    private void FireStart()
    {
        SetFiring();
        FireProjectile();
        Invoke(nameof(FireRecovery), RangedData.FireDuration);
        Invoke(nameof(ResetCanFire), RangedData.FireCooldown);
    }

    private void ResetCanFire()
    {
        _canFire = true;
    }

    protected virtual void FireRecovery()
    {
        StartChase();
    }

    protected void FireProjectile()
    {
        Instantiate(RangedData.ProjectilePrefab, FirePoint.position, FirePoint.rotation);
    }

    protected virtual void DetermineProjectile()
    {
        return;
    }

    protected virtual void StartChase()
    {
        SetChasing();
    }

    protected void SetMeleeData(EnemyMeleeData enemyMeleeData)
    {
        MeleeData = enemyMeleeData;
    }

    protected void SetRangedData(EnemyRangedData enemyRangedData)
    {
        RangedData = enemyRangedData;
    }
}
