using UnityEngine;
using UnityEngine.AI;

public class NecromancerAI : HybridEnemyAI
{
    [Header("ATTACKDATA")]
    [SerializeField] private EnemyMeleeData _meleeData;

    [Header("FIRING")]
    [SerializeField] private EnemyRangedData _multiProjectileData;
    [SerializeField] private EnemyRangedData _trackingProjectileData;
    private int _projectileNumber;

    [Header("MULTI SPELL")]
    [SerializeField] private float _multiRotationIncrement;

    [Header("TELEPORTING")]
    [SerializeField] private GameObject teleportVFX;
    [SerializeField] private float _teleportCooldown;
    [SerializeField] private float minTeleportDistance;
    private Vector3 teleportTargetPosition;
    private bool _canTeleport = true;

    private void Awake()
    {
        SetReferences();
        DetermineProjectile();
        SetMeleeData(_meleeData);
    }

    protected override void Update()
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
                Teleport();
                break;
        }
    }

    protected override void DetermineProjectile()
    {
        _projectileNumber = Random.Range(0, 2);

        if (_projectileNumber == 0)
        {
            SetRangedData(_multiProjectileData);
        }
        else
        {
            SetRangedData(_trackingProjectileData);
        }
    }

    protected override void FireAction()
    {
        if (_projectileNumber == 0)
        {
            FireMultiProjectile();
        }
        else
        {
            FireProjectile();
        }
    }

    private void FireMultiProjectile()
    {
        float currentRotationOffset = -_multiRotationIncrement;

        for (int i = 0; i < 3; i++)
        {
            Quaternion projectileRotation = Quaternion.Euler(FirePoint.eulerAngles.x, FirePoint.eulerAngles.y + currentRotationOffset, FirePoint.eulerAngles.z);

            Instantiate(RangedData.ProjectilePrefab, FirePoint.position, projectileRotation);

            currentRotationOffset += _multiRotationIncrement;
        }
    }

    private void Teleport()
    {
        if (_canTeleport)
        {
            _canTeleport = false;
            Animator.SetFloat("Speed", 0f);

            Instantiate(teleportVFX, transform.position, Quaternion.identity);

            int maxAttempts = 50;
            Vector3 potentialRetreatPosition;
            NavMeshHit hit;

            for (int i = 0; i < maxAttempts; i++)
            {
                Vector3 randomDirection = Random.insideUnitSphere;
                randomDirection.y = 0;
                potentialRetreatPosition = transform.position + randomDirection.normalized * RangedData.FireRange;

                if ((potentialRetreatPosition - transform.position).magnitude < minTeleportDistance)
                {
                    continue;
                }

                if (NavMesh.SamplePosition(potentialRetreatPosition, out hit, RangedData.FireRange, NavMesh.AllAreas))
                {
                    Collider[] colliders = Physics.OverlapBox(hit.position, new Vector3(1f, 1f, 1f), Quaternion.identity, LayerMask.GetMask("Terrain"));

                    if (colliders.Length == 0)
                    {
                        teleportTargetPosition = hit.position;
                        transform.position = hit.position;
                        transform.LookAt(PlayerTransform.position);

                        StartChase();
                        break;
                    }
                }
            }
        }
        else
        {
            StartChase();
        }
    }

    protected override void StartChase()
    {
        SetChasing();
        DetermineProjectile();
        Invoke(nameof(ResetCanTeleport), _teleportCooldown);
    }

    private void ResetCanTeleport()
    {
        _canTeleport = true;
    }
}
