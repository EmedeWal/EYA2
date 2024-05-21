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
    [SerializeField] private float teleportCD;
    [SerializeField] private float minTeleportDistance;
    private Vector3 teleportTargetPosition;
    private bool canTeleport = true;

    private void Awake()
    {
        SetReferences();
        DetermineProjectile();
    }

    protected override void DetermineProjectile()
    {
        _projectileNumber = Random.Range(0, 2);

        if (_projectileNumber == 0)
        {
            RangedData = _multiProjectileData;
        }
        else
        {
            RangedData = _trackingProjectileData;
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
        // Starting at -15 degrees for the first projectile.
        float currentRotationOffset = -_multiRotationIncrement;

        // Shoot three projectiles
        for (int i = 0; i < 3; i++)
        {
            // Calculate the rotation for this projectile
            Quaternion projectileRotation = Quaternion.Euler(FirePoint.eulerAngles.x, spellPoint.eulerAngles.y + currentRotationOffset, spellPoint.eulerAngles.z);

            // Instantiate the projectile with the adjusted rotation
            GameObject projectile = Instantiate(spellPrefab, spellPoint.position, projectileRotation);
            //projectile.GetComponent<ForwardProjectile>().SetDamage(multiDamage);
            projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward * spellForce * 100, ForceMode.Force);

            // Increment the _offset for the next projectile
            currentRotationOffset += multiRotationIncrement;
        }
    }

    private void Teleport()
    {
        // If the enemy can teleport, teleport away
        if (canTeleport)
        {
            // The enemy should play the idle animation
            animator.SetFloat("Speed", 0f);

            // Spawn some VFX for better feedback
            StartCoroutine(TeleportVFX());

            canTeleport = false;
            int maxAttempts = 50;
            Vector3 potentialRetreatPosition = Vector3.zero;
            NavMeshHit hit;

            for (int i = 0; i < maxAttempts;  i++)
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
            // If the enemy cannot teleport, but should, start chasing
            currentState = EnemyState.Chasing;
        }
    }

    private void TeleportReset()
    {
        canTeleport = true;
    }
}
