using UnityEngine;
using UnityEngine.AI;

public abstract class RangedEnemyAI : EnemyAI
{
    [Header("FIRE POINT")]
    [SerializeField] private Transform _firePoint;
    private EnemyRangedData _rangedData;
    private bool _canFire = true;

    [Header("RETREATING")]
    [SerializeField] private float _retreatDistance;
    [SerializeField] private float _safeDistance;
    private Vector3 _retreatTargetPosition;
    private bool _isRetreating;

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

            case EnemyState.Firing:
                Fire();
                break;

            case EnemyState.Retreating:
                RetreatToNewPosition();
                break;
        }
    }

    private void Chase()
    {
        if (InRange(_rangedData.FireRange))
        {
            SetFiring();
        }
        else
        {
            MoveTowardsPlayer();
        }
    }

    private void Fire()
    {
        if (!_canFire) return;

        PlayAnimation(_rangedData.AnimationParameter);
        CancelMovement();
        SetCharging();

        _canFire = false;
        RotationSpeed *= RotationModifier;

        Invoke(nameof(StartFire), _rangedData.ChargeDuration);
    }

    private void StartFire()
    {
        SetFiring();
        FireProjectile();

        Invoke(nameof(SetRetreating), _rangedData.FireDuration);
        Invoke(nameof(ResetCanFire), _rangedData.FireCooldown);

        RotationSpeed /= RotationModifier;
    }

    private void FireProjectile()
    {
        Instantiate(_rangedData.ProjectilePrefab, _firePoint.position, _firePoint.rotation);
    }

    private void ResetCanFire()
    {
        _canFire = true;
    }

    private void RetreatToNewPosition()
    {
        Animator.SetFloat("Speed", Agent.velocity.magnitude);

        if (_isRetreating && (Vector3.Distance(transform.position, _retreatTargetPosition) <= 0.1f))
        {
            CancelRetreat();
        }
        else if (!_isRetreating && (Vector3.Distance(transform.position, PlayerTransform.position) <= _safeDistance))
        {
            _isRetreating = true;

            bool validPositionFound = false;
            int maxAttempts = 30;
            Vector3 potentialRetreatPosition;
            NavMeshHit hit;

            for (int i = 0; i < maxAttempts && !validPositionFound; i++)
            {
                Vector3 retreatDirection = (transform.position - PlayerTransform.position).normalized + Random.insideUnitSphere * 0.3f; // This is randomness to avoid patterns.
                potentialRetreatPosition = transform.position + retreatDirection.normalized * _retreatDistance;

                if (NavMesh.SamplePosition(potentialRetreatPosition, out hit, _retreatDistance, NavMesh.AllAreas))
                {
                    Collider[] colliders = Physics.OverlapBox(hit.position, new Vector3(1f, 1f, 1f), Quaternion.identity, LayerMask.GetMask("Terrain"));

                    if (colliders.Length == 0)
                    {
                        validPositionFound = true;
                        _retreatTargetPosition = hit.position;
                        Agent.SetDestination(hit.position);
                    }
                }
            }

            if (!validPositionFound)
            {
                CancelRetreat();
            }
        }
        else if (!_isRetreating && _canFire)
        {
            SetChasing();
        }
    }

    private void CancelRetreat()
    {
        _isRetreating = false;

        Agent.SetDestination(transform.position);

        SetCharging();
        Invoke(nameof(SetChasing), 1f);
    }

    public abstract void DetermineFireData();

    protected void SetFireData(EnemyRangedData rangedData)
    {
        _rangedData = rangedData;
    }
}
