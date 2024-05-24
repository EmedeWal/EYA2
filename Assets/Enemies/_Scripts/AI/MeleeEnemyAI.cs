using UnityEngine;

public abstract class MeleeEnemyAI : EnemyAI
{
    [Header("ATTACK POINT")]
    [SerializeField] private Transform _attackPoint;
    protected EnemyMeleeData MeleeData;
    private bool _canAttack = true;

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
                Attack();
                break;
        }
    }

    private void Chase()
    {
        if (!_canAttack)
        {
            RotateTowardsPlayer();
        }
        else if (InRange(MeleeData.AttackRange))
        {
            SetAttacking();
        }
        else
        {
            MoveTowardsPlayer();
        }
    }

    private void Attack()
    {
        if (!_canAttack) return;

        CancelMovement();
        ChargeStart();
        SetCharging();

        _canAttack = false;
        RotationSpeed *= RotationModifier;

        Invoke(nameof(StartAttack), MeleeData.ChargeDuration);
    }

    protected virtual void ChargeStart()
    {
        PlayAnimation(MeleeData.AnimationParameter);
    }

    private void StartAttack()
    {
        SetAttacking();
        AttackAction();

        Invoke(nameof(ResetToChase), MeleeData.AttackDuration);
        Invoke(nameof(ResetCanAttack), MeleeData.AttackCooldown);

        RotationSpeed /= RotationModifier;
    }

    private void ResetCanAttack()
    {
        _canAttack = true;
    }

    private void ResetToChase()
    {
        SetChasing();
        DetermineAttackData();
    }

    protected virtual void DetermineAttackData() 
    {
        return; 
    }

    protected virtual void AttackAction()
    {
        DealDamage(CastBox());
    }

    protected Collider[] CastBox()
    {
        return Physics.OverlapBox(_attackPoint.position, MeleeData.AttackSize, _attackPoint.rotation, PlayerLayer);
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

    protected void SetAttackData(EnemyMeleeData meleeData)
    {
        MeleeData = meleeData;
    }

    //private void OnDrawGizmosSelected()
    //{
    //    if (MeleeData != null)
    //    {
    //        Gizmos.color = Color.blue;
    //        Gizmos.matrix = Matrix4x4.TRS(_attackPoint.position, _attackPoint.rotation, Vector3.one);
    //        Gizmos.DrawWireCube(Vector3.zero, MeleeData.AttackSize);
    //    }
    //}
}
