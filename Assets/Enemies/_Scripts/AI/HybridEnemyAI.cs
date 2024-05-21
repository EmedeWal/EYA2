using UnityEngine;
using UnityEngine.AI;

public class HybridEnemyAI : EnemyAI
{
    [Header("ATTACK AND FIRE POINTS")]
    [SerializeField] protected Transform AttackPoint;
    [SerializeField] protected Transform FirePoint;

    protected EnemyMeleeData MeleeData;
    protected EnemyRangedData RangedData;
    private bool _canAttack = true;
    private bool _canFire = true;

    private void Update()
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

            case EnemyState.Retreating:
                //Teleport();
                break;
        }
    }

    private void Chase()
    {
        // If the player is in melee range, attack in melee. Also check if it can attack, otherwise use a ranged spell
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


    private void ChargeAttack()
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

    private void AttackRecovery()
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
                health.TakeDamage(MeleeData.AttackDamage);
            }
        }
    }

    private Collider[] CastBox()
    {
        return Physics.OverlapBox(AttackPoint.position, MeleeData.AttackSize, AttackPoint.rotation, PlayerLayer);
    }

    private void ChargeFire()
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
        Invoke(nameof(FireRecovery), RangedData.FireDuration);
        Invoke(nameof(ResetCanFire), RangedData.FireCooldown);
    }

    protected virtual void FireAction()
    {
        FireProjectile();
    }

    private void ResetCanFire()
    {
        _canFire = true;
    }

    private void FireRecovery()
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

    protected void StartChase()
    {
        SetChasing();
        DetermineProjectile();
        //Invoke(nameof(TeleportReset), teleportCD);
    }

    private void TeleportVFX()
    {
        //GameObject VFX = Instantiate(teleportVFX, transform.position + new Vector3(0, 3, 0), Quaternion.identity);

        //yield return new WaitForSeconds(0.7f);

        //Destroy(VFX);
    }

    // END OF EXECUTION

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireCube(attackPoint.position, meleeSize);
    //}
}
